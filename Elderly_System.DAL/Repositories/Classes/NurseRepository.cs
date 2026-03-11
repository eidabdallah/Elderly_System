using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class NurseRepository : INurseRepository
    {
        private readonly ApplicationDbContext _context;

        public NurseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> CountActiveElderliesAsync()
        {
            return await _context.Elderlies
                .AsNoTracking()
                .CountAsync(e =>
                    e.status == Status.Active &&
                    e.ResidentStays.Any(rs => rs.Status == Status.Active));
        }

        public async Task<List<NurseShiftAssignment>> GetAssignmentsInRangeAsync(DateTime startDate, DateTime endDate)
        {
            var s = startDate.Date;
            var eExclusive = endDate.Date.AddDays(1);

            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Shift)
                .Include(a => a.Nurse)
                .Where(a => a.Date >= s && a.Date < eExclusive)
                .ToListAsync();
        }

        public async Task<NurseShiftAssignment?> GetNurseAssignmentByDateAsync(string nurseId, DateTime date)
        {
            var d = date.Date;
            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Shift)
                .FirstOrDefaultAsync(a => a.NurseId == nurseId && a.Date.Date == d);
        }

        public async Task<List<NurseShiftAssignment>> GetAssignmentsByShiftAndDateAsync(int shiftId, DateTime date)
        {
            var d = date.Date;
            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Nurse)
                .Include(a => a.Shift)
                .Where(a => a.ShiftId == shiftId && a.Date.Date == d)
                .ToListAsync();
        }

        public async Task<List<(int DrugPlanId, int ElderlyId, string ElderlyName,  string MedicineName, DateTime EndDate, Status StockStatus)>>
            GetTodayActivePlansAsync(DateTime today)
        {
            var d = today.Date;

            return await _context.DrugPlans
                .AsNoTracking()
                .Where(dp =>
                    dp.MedicineStatus == Status.Active &&
                    dp.StartDate.Date <= d &&
                    dp.EndDate.Date >= d &&
                    dp.Elderly.status == Status.Active &&
                    dp.Elderly.ResidentStays.Any(rs => rs.Status == Status.Active)
                )
                .Select(dp => new
                {
                    DrugPlanId = dp.Id,
                    ElderlyId = dp.ElderlyId,
                    ElderlyName = dp.Elderly.Name,
                    MedicineName = dp.Medicine.Name,
                    EndDate = dp.EndDate,
                    StockStatus = dp.Status
                })
                .ToListAsync()
                .ContinueWith(t => t.Result.Select(x =>
                    (x.DrugPlanId, x.ElderlyId, x.ElderlyName, x.MedicineName, x.EndDate, x.StockStatus)
                ).ToList());
        }

        public async Task<List<(int DrugPlanId, TimeSpan Time)>> GetPlanTimesAsync(List<int> drugPlanIds)
        {
            if (drugPlanIds == null || drugPlanIds.Count == 0) return new();

            return await _context.DrugPlanTimes
                .AsNoTracking()
                .Where(t => drugPlanIds.Contains(t.DrugPlanId))
                .OrderBy(t => t.Time)
                .Select(t => new { t.DrugPlanId, t.Time })
                .ToListAsync()
                .ContinueWith(t => t.Result.Select(x => (x.DrugPlanId, x.Time)).ToList());
        }

        public async Task<Dictionary<int, int>> GetTodayMedicationCountsByPlanAsync(List<int> drugPlanIds, DateTime start, DateTime end)
        {
            if (drugPlanIds == null || drugPlanIds.Count == 0) return new();

            var rows = await _context.Medications
                .AsNoTracking()
                .Where(m =>
                    drugPlanIds.Contains(m.DrugPlanId) &&
                    m.DateTime >= start &&
                    m.DateTime < end)
                .GroupBy(m => m.DrugPlanId)
                .Select(g => new { DrugPlanId = g.Key, Count = g.Count() })
                .ToListAsync();

            return rows.ToDictionary(x => x.DrugPlanId, x => x.Count);
        }

        public async Task<List<(DateTime DateTime, int DrugPlanId, int ElderlyId, string ElderlyName, string MedicineName, string Dose, string NurseId, string NurseName)>>
            GetTodayMedicationActivityAsync(DateTime start, DateTime end, int take)
        {
            var list = await _context.Medications
                .AsNoTracking()
                .Where(m => m.DateTime >= start && m.DateTime < end)
                .OrderByDescending(m => m.DateTime)
                .Take(take)
                .Select(m => new
                {
                    m.DateTime,
                    DrugPlanId = m.DrugPlanId,
                    ElderlyId = m.DrugPlan.ElderlyId,
                    ElderlyName = m.DrugPlan.Elderly.Name,
                    MedicineName = m.DrugPlan.Medicine.Name,
                    Dose = m.Dose,
                    NurseId = m.NurseId,
                    NurseName = m.Nurse.FullName ?? ""
                })
                .ToListAsync();

            return list.Select(x =>
                (x.DateTime, x.DrugPlanId, x.ElderlyId, x.ElderlyName, x.MedicineName, x.Dose, x.NurseId, x.NurseName)
            ).ToList();
        }

        public async Task<Dictionary<string, string>> GetShiftKeysForNursesOnDateAsync(List<string> nurseIds, DateTime date)
        {
            if (nurseIds == null || nurseIds.Count == 0) return new();

            var d = date.Date;

            var rows = await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Shift)
                .Where(a => a.Date.Date == d && nurseIds.Contains(a.NurseId))
                .Select(a => new { a.NurseId, ShiftKey = a.Shift.ShiftKey })
                .ToListAsync();

            return rows.ToDictionary(x => x.NurseId, x => x.ShiftKey.ToString());
        }
        public async Task<List<NurseShiftAssignment>> GetAssignmentsByDateAsync(DateTime date)
        {
            var d = date.Date;
            var next = d.AddDays(1);

            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Nurse)
                .Include(a => a.Shift)
                .Where(a => a.Date >= d && a.Date < next)
                .ToListAsync();
        }
    }
}
