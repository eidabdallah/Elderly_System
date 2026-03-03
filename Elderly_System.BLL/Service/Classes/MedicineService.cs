using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Medicine;
using Elderly_System.DAL.DTO.Response.Medicine;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _repository;

        public MedicineService(IMedicineRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> AddDrugPlanAsync(AddDrugPlanRequest request)
        {
            var elderlyExists = await _repository.ElderlyExistsAsync(request.ElderlyId);
            if (!elderlyExists)
                return ServiceResult.Failure("المسن غير موجود.");

            bool sentExisting = request.MedicineId.HasValue;
            bool sentNew = request.NewMedicine != null;

            if (!sentExisting && !sentNew)
                return ServiceResult.Failure("يجب اختيار دواء موجود أو إدخال دواء جديد.");

            if (sentExisting && sentNew)
                return ServiceResult.Failure("يرجى اختيار دواء واحد فقط (موجود أو جديد).");

            if (request.EndDate.Date < request.StartDate.Date)
                return ServiceResult.Failure("تاريخ النهاية يجب أن يكون بعد/يساوي تاريخ البداية.");

            if (request.Times == null || request.Times.Count == 0)
                return ServiceResult.Failure("مواعيد الجرعات مطلوبة.");

            var distinctTimes = request.Times.Distinct().ToList();
            if (distinctTimes.Count != request.Times.Count)
                return ServiceResult.Failure("مواعيد الجرعات تحتوي على تكرار.");

            if (distinctTimes.Count != request.DailyIntake)
                return ServiceResult.Failure("عدد مواعيد الجرعات يجب أن يساوي عدد الجرعات اليومية.");

            Medicine medicine;

            if (request.MedicineId.HasValue)
            {
                var existing = await _repository.GetMedicineByIdAsync(request.MedicineId.Value);
                if (existing == null)
                    return ServiceResult.Failure("الدواء غير موجود.");

                medicine = existing;
            }
            else
            {
                var name = request.NewMedicine!.Name?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                    return ServiceResult.Failure("اسم الدواء لا يمكن أن يكون فارغًا.");

                name = name.Trim();
                var already = await _repository.GetMedicineByNameAndTypeAsync(name, request.NewMedicine.Type);
                if (already != null)
                    return ServiceResult.Failure("هذا الدواء موجود مسبقًا (بنفس الاسم والنوع). يرجى البحث عنه واختياره من القائمة.");
                else
                {
                    medicine = new Medicine
                    {
                        Name = name!,
                        Type = request.NewMedicine.Type
                    };

                    await _repository.AddMedicineAsync(medicine);
                }
            }

            var drugPlan = new DrugPlan
            {
                ElderlyId = request.ElderlyId,
                MedicineId = medicine.Id,
                StartDate = request.StartDate.Date,
                EndDate = request.EndDate.Date,
                DailyIntake = request.DailyIntake,
                Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
                Status = Status.Active,
                DrugPlanTimes = distinctTimes.Select(t => new DrugPlanTime
                {
                    Time = t
                }).ToList()
            };

            await _repository.AddDrugPlanAsync(drugPlan);

            return ServiceResult.SuccessMessage("تم إضافة خطة الدواء بنجاح.");
        }
        public async Task<List<MedicineLookupResponse>> GetAllMedicinesAsync(string? search, int? type)
        {
            var medicines = await _repository.GetAllMedicinesAsync(search, type);

            return medicines.Select(m => new MedicineLookupResponse
            {
                Id = m.Id,
                Name = m.Name,
                TypeName = ((int)m.Type) == 1 ? "حبوب" : "سائل"
            }).ToList();
        }
        public async Task<List<ElderlyDrugPlanResponse>?> GetElderlyDrugPlansAsync(int elderlyId)
        {
            var exists = await _repository.ElderlyExistsAsync(elderlyId);
            if (!exists) return null;

            var plans = await _repository.GetDrugPlansByElderlyIdAsync(elderlyId);

            return plans.Select(dp => new ElderlyDrugPlanResponse
            {
                DrugPlanId = dp.Id,
                MedicineId = dp.MedicineId,
                MedicineName = dp.Medicine.Name,
                MedicineTypeName = dp.Medicine.Type switch
                {
                    MedicineType.Tablet => "حبوب",
                    MedicineType.Syrup => "سائل",
                    _ => "غير محدد"
                },
                StartDate = dp.StartDate.ToString("dd/MM/yyyy"),
                EndDate = dp.EndDate.ToString("dd/MM/yyyy"),
                DailyIntake = dp.DailyIntake,
                Notes = dp.Notes,
                Status = dp.Status,
                MedicineStatus = dp.MedicineStatus,
                Times = dp.DrugPlanTimes
                    .OrderBy(t => t.Time)
                    .Select(t => t.Time.ToString(@"hh\:mm"))
                    .ToList()
            }).ToList();
        }
        public async Task<ServiceResult> UpdateDrugPlanAsync(int drugPlanId, DrugPlanUpdateRequest request)
        {
            var dp = await _repository.GetDrugPlanWithTimesAsync(drugPlanId);
            if (dp == null)
                return ServiceResult.Failure("خطة الدواء غير موجودة.");

            bool sentAny =
                request.StartDate.HasValue ||
                request.EndDate.HasValue ||
                request.DailyIntake.HasValue ||
                request.Notes != null ||
                request.Times != null;

            if (!sentAny)
                return ServiceResult.Failure("لم يتم إرسال أي بيانات للتعديل.");

            var newStart = request.StartDate?.Date ?? dp.StartDate.Date;
            var newEnd = request.EndDate?.Date ?? dp.EndDate.Date;

            if (newEnd < newStart)
                return ServiceResult.Failure("تاريخ النهاية يجب أن يكون بعد/يساوي تاريخ البداية.");

            var newDailyIntake = request.DailyIntake ?? dp.DailyIntake;

            if (request.Times != null)
            {
                if (request.Times.Count == 0)
                    return ServiceResult.Failure("مواعيد الجرعات لا يمكن أن تكون فارغة.");

                var distinctTimes = request.Times.Distinct().ToList();
                if (distinctTimes.Count != request.Times.Count)
                    return ServiceResult.Failure("مواعيد الجرعات تحتوي على تكرار.");

                if (distinctTimes.Count != newDailyIntake)
                    return ServiceResult.Failure("عدد مواعيد الجرعات يجب أن يساوي عدد الجرعات اليومية.");
            }
            else
            {
                if (request.DailyIntake.HasValue)
                {
                    var currentCount = dp.DrugPlanTimes?.Count ?? 0;
                    if (currentCount != newDailyIntake)
                        return ServiceResult.Failure("تم تعديل عدد الجرعات اليومية. يرجى إرسال مواعيد الجرعات لتتطابق مع العدد الجديد.");
                }
            }

            if (request.StartDate.HasValue) dp.StartDate = newStart;
            if (request.EndDate.HasValue) dp.EndDate = newEnd;
            if (request.DailyIntake.HasValue) dp.DailyIntake = newDailyIntake;

            if (request.Notes != null)
                dp.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();

            bool planChanged =
                request.StartDate.HasValue ||
                request.EndDate.HasValue ||
                request.DailyIntake.HasValue ||
                request.Notes != null;

            if (planChanged)
                await _repository.UpdateDrugPlanAsync(dp);

            if (request.Times != null)
            {
                var newTimes = request.Times
                    .Distinct()
                    .Select(t => new DrugPlanTime
                    {
                        DrugPlanId = dp.Id,
                        Time = t
                    }).ToList();

                await _repository.ReplaceDrugPlanTimesAsync(dp.Id, newTimes);
            }

            return ServiceResult.SuccessMessage("تم تعديل خطة الدواء بنجاح.");
        }

        public async Task<ServiceResult> UpdateDrugPlanStatusAsync(int drugPlanId, int status)
        {
            if (status != 1 && status != 2 && status != 3)
                return ServiceResult.Failure("قيمة الحالة غير صحيحة.");

            var dp = await _repository.GetDrugPlanWithTimesAsync(drugPlanId);
            if (dp == null)
                return ServiceResult.Failure("خطة الدواء غير موجودة.");

            dp.Status = (Status)status;
            await _repository.UpdateDrugPlanAsync(dp);

            return ServiceResult.SuccessMessage("تم تعديل حالة خطة الدواء بنجاح.");
        }
        public async Task<ServiceResult> UpdateDrugPlanStatusMedAsync(int drugPlanId, int status)
        {
            if (status != 1 && status != 2)
                return ServiceResult.Failure("قيمة الحالة غير صحيحة.");

            var dp = await _repository.GetDrugPlanWithTimesAsync(drugPlanId);
            if (dp == null)
                return ServiceResult.Failure("خطة الدواء غير موجودة.");

            dp.MedicineStatus = (Status)status;
            await _repository.UpdateDrugPlanAsync(dp);

            return ServiceResult.SuccessMessage("تم تعديل حالة خطة الدواء بنجاح.");
        }
        public async Task<ServiceResult> AddMedicationAsync(MedicationCreateRequest request, string nurseId)
        {
            if (string.IsNullOrWhiteSpace(nurseId))
                return ServiceResult.Failure("تعذر تحديد الممرضة من التوكن.");

            var plan = await _repository.GetDrugPlanByIdAsync(request.DrugPlanId);
            if (plan == null)
                return ServiceResult.Failure("الخطة الدوائية غير موجودة.");

            if (plan.ElderlyId != request.ElderlyId)
                return ServiceResult.Failure("هذه الخطة غير تابعة للمسن المحدد.");

            var medDate = DateTime.Now.Date;
            if (medDate < plan.StartDate.Date || medDate > plan.EndDate.Date)
                return ServiceResult.Failure("لا يمكن إضافة الجرعة لأن التاريخ خارج فترة الخطة الدوائية.");

            var countToday = await _repository.CountMedicationsForPlanOnDateAsync(plan.Id, medDate);

            if (countToday >= plan.DailyIntake)
                return ServiceResult.Failure("لا يمكن إضافة جرعة جديدة: تم تسجيل جميع جرعات اليوم لهذا الدواء.");

            var medication = new Medication
            {
                DrugPlanId = plan.Id,
                NurseId = nurseId,
                DateTime = DateTime.Now,
                Dose = request.Dose.Trim()
            };

            await _repository.AddMedicationAsync(medication);

            return ServiceResult.SuccessMessage("تم تسجيل الجرعة بنجاح.");
        }
        public async Task<List<ElderlyDrugPlanResponse>?> GetElderlyMedicineAsync(int elderlyId)
        {
            var exists = await _repository.ElderlyExistsAsync(elderlyId);
            if (!exists) return null;

            var plans = await _repository.GetDrugPlansByElderlyIdAsync(elderlyId);

            return plans.Select(dp => new ElderlyDrugPlanResponse
            {
                DrugPlanId = dp.Id,
                MedicineId = dp.MedicineId,
                MedicineName = dp.Medicine.Name,
                MedicineTypeName = dp.Medicine.Type switch
                {
                    MedicineType.Tablet => "حبوب",
                    MedicineType.Syrup => "سائل",
                    _ => "غير محدد"
                },
            }).ToList();
        }
        public async Task<ServiceResult> GetElderlyWeeklyMedicationScheduleAsync(int elderlyId, int offset = 0)
        {
            var exists = await _repository.ElderlyExistsAsync(elderlyId);
            if (!exists)
                return ServiceResult.Failure("المسن غير موجود.");

            var baseDate = DateTime.Now.Date.AddDays(offset * 7);
            var start = GetSaturdayStart(baseDate);

            var dates = Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToList();
            var startDate = dates.First().Date;
            var endDate = dates.Last().Date;

            var dateKeys = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            // 1) خطط الأدوية النشطة والمتداخلة مع الأسبوع
            var plans = await _repository.GetActiveDrugPlansForElderlyInRangeAsync(elderlyId, startDate, endDate);

            // 2) كل الجرعات المسجلة خلال الأسبوع (لهذه المسنة)
            var meds = await _repository.GetMedicationsForElderlyInRangeAsync(elderlyId, startDate, endDate);

            // Group: (DrugPlanId, Date)
            var medsMap = meds
                .GroupBy(m => new { m.DrugPlanId, Day = m.DateTime.Date })
                .ToDictionary(g => (g.Key.DrugPlanId, g.Key.Day), g => g.ToList());

            string TypeName(MedicineType t) => t switch
            {
                MedicineType.Tablet => "حبوب",
                MedicineType.Syrup => "سائل",
                _ => "غير محدد"
            };

            var rows = new List<ElderlyWeeklyMedicationRowDto>();

            foreach (var dp in plans)
            {
                var row = new ElderlyWeeklyMedicationRowDto
                {
                    DrugPlanId = dp.Id,
                    MedicineName = dp.Medicine?.Name ?? "",
                    MedicineTypeName = dp.Medicine != null ? TypeName(dp.Medicine.Type) : "",
                    DailyIntake = dp.DailyIntake,
                    ScheduledTimes = dp.DrugPlanTimes
                        .OrderBy(t => t.Time)
                        .Select(t => t.Time.ToString(@"hh\:mm"))
                        .ToList()
                };

                var days = new Dictionary<string, ElderlyMedicationDayCellDto>();

                foreach (var d in dates)
                {
                    var key = d.ToString("yyyy-MM-dd");
                    var inPeriod = d.Date >= dp.StartDate.Date && d.Date <= dp.EndDate.Date;

                    if (!inPeriod)
                    {
                        days[key] = new ElderlyMedicationDayCellDto
                        {
                            InPlanPeriod = false,
                            TakenCount = 0,
                            RequiredCount = dp.DailyIntake,
                            Taken = new List<MedicationTakenDto>(),
                            Summary = "-"
                        };
                        continue;
                    }

                    medsMap.TryGetValue((dp.Id, d.Date), out var dayMeds);
                    dayMeds ??= new List<Medication>();

                    var takenList = dayMeds
                        .OrderBy(x => x.DateTime)
                        .Select(x => new MedicationTakenDto
                        {
                            Time = x.DateTime.ToString("HH:mm"),
                            Dose = x.Dose
                        })
                        .ToList();

                    var takenCount = takenList.Count;
                    var required = dp.DailyIntake;

                    days[key] = new ElderlyMedicationDayCellDto
                    {
                        InPlanPeriod = true,
                        TakenCount = takenCount,
                        RequiredCount = required,
                        Taken = takenList,
                        Summary = $"{takenCount}/{required}"
                    };
                }

                row.Days = days;
                rows.Add(row);
            }

            var response = new ElderlyWeeklyMedicationScheduleResponse
            {
                Dates = dateKeys,
                Rows = rows
            };

            return ServiceResult.SuccessWithData(response, "تم جلب جدول الأدوية الأسبوعي بنجاح");
        }

        private static DateTime GetSaturdayStart(DateTime anyDate)
        {
            var d = anyDate.Date;
            int diff = ((int)d.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return d.AddDays(-diff);
        }
    }
}

