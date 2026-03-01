using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Nurse;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.BLL.Service.Classes
{
    public class NurseShiftService : INurseShiftService
    {
        private readonly INurseShiftRepository _repository;
        private readonly ApplicationDbContext _context;

        public NurseShiftService(INurseShiftRepository repository , ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<ServiceResult> GetActiveNursesAsync()
        {
            var nurses = await _repository.GetActiveNursesAsync();

            var data = nurses.Select(n => new ActiveNurseMiniDto
            {
                Id = n.Id,
                FullName = n.FullName ?? ""
            }).ToList();

            return ServiceResult.SuccessWithData(data, "تم جلب الممرضات النشطات بنجاح");
        }

        public async Task<ServiceResult> AssignDailyShiftsAsync(AssignDailyShiftsRequest request)
        {
            if (request == null)
                return ServiceResult.Failure("البيانات غير صحيحة.");

            var date = request.Date.Date;

            var today = DateTime.UtcNow.Date; 
            if (date < today)
                return ServiceResult.Failure("التاريخ يجب أن يكون اليوم أو بعده. لا يمكن اختيار تاريخ سابق.");

            static List<string> Clean(List<string> ids) =>
                (ids ?? new List<string>())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

            var a = Clean(request.ANurseIds);
            var b = Clean(request.BNurseIds);
            var c = Clean(request.CNurseIds);

            if (a.Count != 2) return ServiceResult.Failure("شفت A يجب أن يحتوي على ممرضتين .");
            if (b.Count != 2) return ServiceResult.Failure("شفت B يجب أن يحتوي على ممرضتين .");
            if (c.Count != 2) return ServiceResult.Failure("شفت C يجب أن يحتوي على ممرضتين .");

            var all = a.Concat(b).Concat(c).ToList();
            if (all.Distinct().Count() != 6)
                return ServiceResult.Failure("ممنوع تكرار الممرضات. يجب اختيار 6 ممرضات مختلفات.");

            var activeNurses = await _repository.GetActiveNursesByIdsAsync(all);
            if (activeNurses.Count != 6)
                return ServiceResult.Failure("يوجد ممرضة/ممرضات غير نشطات أو غير موجودات ضمن الاختيار.");

            var existing = await _repository.GetAssignmentsByDateAndNurseIdsAsync(date, all);
            if (existing.Any())
                return ServiceResult.Failure("تم اختيار شفتات الممرضين لهذا اليوم.");

            var shifts = await _repository.GetShiftsByKeysAsync(new List<char> { 'A', 'B', 'C' });
            if (!shifts.ContainsKey('A') || !shifts.ContainsKey('B') || !shifts.ContainsKey('C'))
                return ServiceResult.Failure("الشفتات غير موجودة في النظام .");

            var assignments = new List<NurseShiftAssignment>();

            assignments.AddRange(a.Select(nId => new NurseShiftAssignment
            {
                NurseId = nId,
                ShiftId = shifts['A'].Id,
                Date = date
            }));

            assignments.AddRange(b.Select(nId => new NurseShiftAssignment
            {
                NurseId = nId,
                ShiftId = shifts['B'].Id,
                Date = date
            }));

            assignments.AddRange(c.Select(nId => new NurseShiftAssignment
            {
                NurseId = nId,
                ShiftId = shifts['C'].Id,
                Date = date
            }));

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                await _repository.AddAssignmentsAsync(assignments);

                var saved = await _repository.SaveChangesAsync();
                if (saved <= 0)
                {
                    await tx.RollbackAsync();
                    return ServiceResult.Failure("حدث خطأ أثناء حفظ الشفتات.");
                }

                await tx.CommitAsync();
                return ServiceResult.SuccessMessage("تم تعيين شفتات اليوم بنجاح.");
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync();
                return ServiceResult.Failure("تعذر الحفظ بسبب تعارض بالبيانات (قد تكون ممرضة تم تعيين شفت لها بنفس اليوم).");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return ServiceResult.Failure($"حدث خطأ غير متوقع: {ex.Message}");
            }
        }
        public async Task<ServiceResult> GetScheduleAsync(string? view = "week", DateTime? date = null, int offset = 0)
        {
            var baseDate = (date ?? DateTime.Now).Date;

            bool isDayView = string.Equals(view, "day", StringComparison.OrdinalIgnoreCase);

            if (!isDayView)
                baseDate = baseDate.AddDays(offset * 7);

            List<DateTime> dates;
            if (isDayView)
            {
                dates = new List<DateTime> { baseDate };
            }
            else
            {
                var start = GetSaturdayStart(baseDate);
                dates = Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToList();
            }

            var startDate = dates.First().Date;
            var endDate = dates.Last().Date;

            var dateKeys = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            var nurses = await _repository.GetActiveNursesAsync();
            var assignments = await _repository.GetAssignmentsInRangeAsync(startDate, endDate);

            var map = assignments.ToDictionary(
                a => (a.NurseId, a.Date.Date),
                a => a.Shift.ShiftKey.ToString()
            );

            var scheduledDays = assignments
                .Select(a => a.Date.Date)
                .ToHashSet();

            var rows = new List<NurseShiftScheduleRowDto>();

            foreach (var n in nurses)
            {
                var days = new Dictionary<string, string>();

                foreach (var d in dates)
                {
                    var key = d.ToString("yyyy-MM-dd");

                    if (map.TryGetValue((n.Id, d.Date), out var shiftVal))
                    {
                        days[key] = shiftVal; 
                    }
                    else
                    {
                        days[key] = scheduledDays.Contains(d.Date) ? "عطلة" : "-";
                    }
                }

                rows.Add(new NurseShiftScheduleRowDto
                {
                    NurseId = n.Id,
                    NurseName = n.FullName ?? "",
                    Days = days
                });
            }

            var response = new NurseShiftScheduleResponse
            {
                Dates = dateKeys,
                Rows = rows
            };

            return ServiceResult.SuccessWithData(response, "تم جلب جدول الشفتات بنجاح");
        }

        public async Task<ServiceResult> GetMyWeeklyScheduleAsync(string nurseId, int offset = 0)
        {
            var nurse = await _repository.GetActiveNurseByIdAsync(nurseId);
            if (nurse is null)
                return ServiceResult.Failure("الممرضة غير موجودة أو غير نشطة.");

            var baseDate = DateTime.Now.Date.AddDays(offset * 7);

            var start = GetSaturdayStart(baseDate);
            var dates = Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToList();

            var startDate = dates.First().Date;
            var endDate = dates.Last().Date;

            var dateKeys = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            var nurseAssignments = await _repository.GetAssignmentsInRangeByNurseAsync(nurseId, startDate, endDate);

            var scheduledDaysList = await _repository.GetScheduledDaysInRangeAsync(startDate, endDate);
            var scheduledDays = scheduledDaysList.ToHashSet();

            var nurseMap = nurseAssignments.ToDictionary(
                a => a.Date.Date,
                a => a.Shift.ShiftKey.ToString()
            );

            var days = new Dictionary<string, string>();

            foreach (var d in dates)
            {
                var key = d.ToString("yyyy-MM-dd");

                if (nurseMap.TryGetValue(d.Date, out var shiftVal))
                {
                    days[key] = shiftVal;         
                }
                else
                {
                    days[key] = scheduledDays.Contains(d.Date) ? "عطلة" : "-";
                }
            }

            var response = new NurseShiftScheduleResponse
            {
                Dates = dateKeys,
                Rows = new List<NurseShiftScheduleRowDto>
        {
            new NurseShiftScheduleRowDto
            {
                NurseId = nurse.Id,
                NurseName = nurse.FullName ?? "",
                Days = days
            }
        }
            };

            return ServiceResult.SuccessWithData(response, "تم جلب جدولك الأسبوعي بنجاح");
        }


        private static DateTime GetSaturdayStart(DateTime anyDate)
        {
            var d = anyDate.Date;
            int diff = ((int)d.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return d.AddDays(-diff);
        }
       


    }
}
