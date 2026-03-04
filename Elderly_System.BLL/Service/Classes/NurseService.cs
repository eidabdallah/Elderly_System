using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class NurseService : INurseService
    {
        private readonly INurseRepository _repository;

        public NurseService(INurseRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> GetHomeAsync(string nurseId, int graceMinutes = 30, int reminderMinutes = 10,int expiringDays = 3, int activityTake = 20)
        {
            if (string.IsNullOrWhiteSpace(nurseId))
                return ServiceResult.Failure("تعذر تحديد الممرضة من التوكن.");

            var now = DateTime.Now;
            var today = now.Date;
            var tomorrow = today.AddDays(1);

            var activeCount = await _repository.CountActiveElderliesAsync();

            var weekStart = GetSaturdayStart(today);
            var weekEnd = weekStart.AddDays(6);

            var weekAssignments = await _repository.GetAssignmentsInRangeAsync(weekStart, weekEnd);

            var dates = Enumerable.Range(0, 7).Select(i => weekStart.AddDays(i)).ToList();
            var dateKeys = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            var scheduledDays = weekAssignments.Select(a => a.Date.Date).ToHashSet();

            var nurseMap = weekAssignments
                .Where(a => a.NurseId == nurseId)
                .ToDictionary(a => a.Date.Date, a => a.Shift.ShiftKey.ToString());

            var weeklyDays = new Dictionary<string, string>();
            foreach (var d in dates)
            {
                var key = d.ToString("yyyy-MM-dd");

                if (nurseMap.TryGetValue(d.Date, out var shiftVal))
                    weeklyDays[key] = shiftVal;
                else
                    weeklyDays[key] = scheduledDays.Contains(d.Date) ? "عطلة" : "-";
            }

            var weeklyShift = new NurseWeeklyShiftDto
            {
                Dates = dateKeys,
                Days = weeklyDays
            };

            var todayAssignment = await _repository.GetNurseAssignmentByDateAsync(nurseId, today);

            string todayShiftKey = todayAssignment != null
                ? todayAssignment.Shift.ShiftKey.ToString()
                : (scheduledDays.Contains(today) ? "عطلة" : "-");

            var todayTeam = new List<NurseMiniDto>();
            if (todayAssignment != null)
            {
                var sameShift = await _repository.GetAssignmentsByShiftAndDateAsync(todayAssignment.ShiftId, today);
                todayTeam = sameShift
                    .Where(a => a.NurseId != nurseId)
                    .Select(a => new NurseMiniDto
                    {
                        Id = a.NurseId,
                        FullName = a.Nurse?.FullName ?? "",
                        ShiftKey = a.Shift?.ShiftKey.ToString() ?? "-"
                    })
                    .OrderBy(x => x.FullName)
                    .ToList();
            }

            var plans = await _repository.GetTodayActivePlansAsync(today);
            var planIds = plans.Select(p => p.DrugPlanId).Distinct().ToList();

            var times = await _repository.GetPlanTimesAsync(planIds);
            var timesMap = times
                .GroupBy(x => x.DrugPlanId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Time).OrderBy(t => t).ToList());

            var takenCounts = await _repository.GetTodayMedicationCountsByPlanAsync(planIds, today, tomorrow);

            var overdueAlerts = new List<NurseOverdueDoseAlertDto>();
            var grace = TimeSpan.FromMinutes(Math.Max(0, graceMinutes));
            var nowTime = now.TimeOfDay;

            foreach (var p in plans)
            {
                if (!timesMap.TryGetValue(p.DrugPlanId, out var planTimes) || planTimes.Count == 0)
                    continue;

                var dueCutoff = nowTime - grace;
                var dueTimes = planTimes.Where(t => t <= dueCutoff).ToList();
                if (dueTimes.Count == 0) continue;

                takenCounts.TryGetValue(p.DrugPlanId, out var takenToday);

                var expectedByNow = dueTimes.Count;

                if (takenToday < expectedByNow)
                {
                    var missingIndex = takenToday; 
                    var due = dueTimes[Math.Min(missingIndex, dueTimes.Count - 1)];

                    var lateMinutes = (int)Math.Max(0, (nowTime - due).TotalMinutes);

                    overdueAlerts.Add(new NurseOverdueDoseAlertDto
                    {
                        ElderlyId = p.ElderlyId,
                        ElderlyName = p.ElderlyName,

                        DrugPlanId = p.DrugPlanId,
                        MedicineName = p.MedicineName,

                        DueTime = due.ToString(@"hh\:mm"),
                        LateMinutes = lateMinutes,
                        Message = $"جرعة متأخرة: {p.ElderlyName}  - {p.MedicineName} (وقت {due:hh\\:mm})"
                    });
                }
            }

            overdueAlerts = overdueAlerts
                .OrderByDescending(x => x.LateMinutes)
                .ToList();

            var dueSoonAlerts = new List<NurseDueSoonDoseAlertDto>();
            var reminderWindow = TimeSpan.FromMinutes(Math.Max(0, reminderMinutes));

            foreach (var p in plans)
            {
                if (!timesMap.TryGetValue(p.DrugPlanId, out var planTimes) || planTimes.Count == 0)
                    continue;

                takenCounts.TryGetValue(p.DrugPlanId, out var takenToday);

                // الأوقات القادمة خلال نافذة التذكير
                var upcoming = planTimes
                    .Where(t => t > nowTime && (t - nowTime) <= reminderWindow)
                    .OrderBy(t => t)
                    .ToList();

                if (upcoming.Count == 0) continue;

                foreach (var due in upcoming)
                {
                    // محاولة بسيطة لتجنب التذكير إذا الجرعة "ممكن" تكون انأخذت بدري:
                    // كم جرعة مفروض تكون مأخوذة بحلول وقت due
                    var expectedByDue = planTimes.Count(t => t <= due);

                    if (takenToday >= expectedByDue)
                        continue;

                    var minutesLeft = (int)Math.Ceiling((due - nowTime).TotalMinutes);
                    if (minutesLeft < 0) continue;

                    var dueText = due.ToString(@"hh\:mm");
                    var key = $"{p.DrugPlanId}-{today:yyyyMMdd}-{dueText.Replace(":", "")}";

                    dueSoonAlerts.Add(new NurseDueSoonDoseAlertDto
                    {
                        ElderlyId = p.ElderlyId,
                        ElderlyName = p.ElderlyName,
                        DrugPlanId = p.DrugPlanId,
                        MedicineName = p.MedicineName,
                        DueTime = dueText,
                        MinutesLeft = minutesLeft,
                        ReminderKey = key,
                        Message = $"تذكير: جرعة قريبة خلال {minutesLeft} دقيقة — {p.ElderlyName} - {p.MedicineName} (الوقت {dueText})"
                    });
                }
            }

            dueSoonAlerts = dueSoonAlerts
                .OrderBy(x => x.MinutesLeft)
                .ThenBy(x => x.ElderlyName)
                .ToList();

            expiringDays = Math.Max(0, expiringDays);

            var expiringAlerts = plans
                .Select(p => new
                {
                    Plan = p,
                    DaysLeft = (p.EndDate.Date - today).Days
                })
                .Where(x => x.DaysLeft >= 0 && x.DaysLeft <= expiringDays)
                .OrderBy(x => x.DaysLeft)
                .Select(x => new NursePlanExpiringAlertDto
                {
                    ElderlyId = x.Plan.ElderlyId,
                    ElderlyName = x.Plan.ElderlyName,

                    DrugPlanId = x.Plan.DrugPlanId,
                    MedicineName = x.Plan.MedicineName,

                    EndDate = x.Plan.EndDate.ToString("yyyy-MM-dd"),
                    DaysLeft = x.DaysLeft,
                    Message = $"خطة الدواء قربت تنتهي: {x.Plan.ElderlyName} - {x.Plan.MedicineName} (متبقي {x.DaysLeft} يوم)"
                })
                .ToList();

            var stockAlerts = plans
                .Where(p => (int)p.StockStatus == 2 || (int)p.StockStatus == 3)
                .OrderBy(p => (int)p.StockStatus) 
                .Select(p =>
                {
                    var st = (int)p.StockStatus;
                    var stText = st == 2 ? "قرب الانتهاء" : "منتهي";

                    return new NurseStockAlertDto
                    {
                        ElderlyId = p.ElderlyId,
                        ElderlyName = p.ElderlyName,

                        DrugPlanId = p.DrugPlanId,
                        MedicineName = p.MedicineName,

                        StockStatus = st,
                        StockStatusText = stText,
                        Message = $"حالة كمية الدواء: {p.ElderlyName} - {p.MedicineName} ({stText})"
                    };
                })
                .ToList();

            activityTake = Math.Max(0, activityTake);
            var rawActivity = await _repository.GetTodayMedicationActivityAsync(today, tomorrow, activityTake);

            var nurseIdsInActivity = rawActivity.Select(a => a.NurseId).Distinct().ToList();
            var shiftMap = await _repository.GetShiftKeysForNursesOnDateAsync(nurseIdsInActivity, today);

            var activity = rawActivity
                .Select(a => new NurseTodayActivityDto
                {
                    Time = a.DateTime.ToString("HH:mm"),
                    ElderlyId = a.ElderlyId,
                    ElderlyName = a.ElderlyName,

                    DrugPlanId = a.DrugPlanId,
                    MedicineName = a.MedicineName,
                    Dose = a.Dose,

                    NurseId = a.NurseId,
                    NurseName = a.NurseName,
                    ShiftKey = shiftMap.TryGetValue(a.NurseId, out var sk) ? sk : "-"
                })
                .ToList();

            var response = new NurseHomeDashboardResponse
            {
                ActiveElderliesCount = activeCount,
                MyWeeklyShift = weeklyShift,

                TodayShiftKey = todayShiftKey,
                TodayTeam = todayTeam,
                DueSoonDoses = dueSoonAlerts,
                OverdueDoses = overdueAlerts,
                PlansExpiringSoon = expiringAlerts,
                LowOrOutStock = stockAlerts,

                TodayActivity = activity
            };

            return ServiceResult.SuccessWithData(response, "تم جلب الصفحة الرئيسية للممرضة بنجاح");
        }

        private static DateTime GetSaturdayStart(DateTime anyDate)
        {
            var d = anyDate.Date;
            int diff = ((int)d.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return d.AddDays(-diff);
        }
    }
}
