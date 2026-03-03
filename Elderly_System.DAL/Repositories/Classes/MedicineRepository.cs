using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicineRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ElderlyExistsAsync(int elderlyId)
        {
            return await _context.Elderlies.AnyAsync(e => e.Id == elderlyId);
        }

        public async Task<Medicine?> GetMedicineByIdAsync(int id)
        {
            return await _context.Medicines.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Medicine?> GetMedicineByNameAndTypeAsync(string name, MedicineType type)
        {
            var trimmed = name.Trim().ToLower();

            return await _context.Medicines
                .FirstOrDefaultAsync(m =>
                    m.Type == type &&
                    m.Name.Trim().ToLower() == trimmed);
        }

        public async Task AddMedicineAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task AddDrugPlanAsync(DrugPlan drugPlan)
        {
            await _context.DrugPlans.AddAsync(drugPlan);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Medicine>> GetAllMedicinesAsync(string? search, int? type)
        {
            var query = _context.Medicines.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(m => m.Name.Contains(s));
            }

            if (type.HasValue)
                query = query.Where(m => (int)m.Type == type.Value);

            return await query
                .OrderBy(m => m.Name)
                .ToListAsync();
        }
        public async Task<List<DrugPlan>> GetDrugPlansByElderlyIdAsync(int elderlyId)
        {
            return await _context.DrugPlans
                .Where(dp => dp.ElderlyId == elderlyId)
                .Include(dp => dp.Medicine)
                .Include(dp => dp.DrugPlanTimes)
                .OrderByDescending(dp => dp.StartDate)
                .ToListAsync();
        }
        public async Task<DrugPlan?> GetDrugPlanWithTimesAsync(int drugPlanId)
        {
            return await _context.DrugPlans
                .Include(dp => dp.DrugPlanTimes)
                .FirstOrDefaultAsync(dp => dp.Id == drugPlanId);
        }

        public async Task UpdateDrugPlanAsync(DrugPlan drugPlan)
        {
            _context.DrugPlans.Update(drugPlan);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceDrugPlanTimesAsync(int drugPlanId, List<DrugPlanTime> newTimes)
        {
            var oldTimes = await _context.DrugPlanTimes
                .Where(t => t.DrugPlanId == drugPlanId)
                .ToListAsync();

            if (oldTimes.Count > 0)
                _context.DrugPlanTimes.RemoveRange(oldTimes);

            await _context.DrugPlanTimes.AddRangeAsync(newTimes);
            await _context.SaveChangesAsync();
        }
        public async Task<DrugPlan?> GetDrugPlanByIdAsync(int drugPlanId)
        {
            return await _context.DrugPlans
                .FirstOrDefaultAsync(dp => dp.Id == drugPlanId);
        }

        public async Task<int> CountMedicationsForPlanOnDateAsync(int drugPlanId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.Medications
                .CountAsync(m =>
                    m.DrugPlanId == drugPlanId &&
                    m.DateTime >= start &&
                    m.DateTime < end);
        }

        public async Task AddMedicationAsync(Medication medication)
        {
            await _context.Medications.AddAsync(medication);
            await _context.SaveChangesAsync();
        }
        public async Task<List<DrugPlan>> GetMedicineByElderlyIdAsync(int elderlyId)
        {
            return await _context.DrugPlans
                .Where(dp => dp.ElderlyId == elderlyId && dp.MedicineStatus == Status.Active)
                .Include(dp => dp.Medicine)
                .ToListAsync();
        }
        public async Task<List<DrugPlan>> GetActiveDrugPlansForElderlyInRangeAsync(int elderlyId, DateTime startDate, DateTime endDate)
        {
            var s = startDate.Date;
            var e = endDate.Date;

            return await _context.DrugPlans
                .Where(dp =>
                    dp.ElderlyId == elderlyId &&
                    dp.MedicineStatus == Status.Active &&
                    dp.StartDate.Date <= e &&
                    dp.EndDate.Date >= s
                )
                .Include(dp => dp.Medicine)
                .Include(dp => dp.DrugPlanTimes)
                .OrderBy(dp => dp.Medicine.Name)
                .ToListAsync();
        }

        public async Task<List<Medication>> GetMedicationsForElderlyInRangeAsync(int elderlyId, DateTime startDate, DateTime endDate)
        {
            var s = startDate.Date;
            var eExclusive = endDate.Date.AddDays(1);

            return await _context.Medications
                .Include(m => m.DrugPlan)
                .Include(m => m.Nurse)
                .Where(m =>
                    m.DrugPlan.ElderlyId == elderlyId &&
                    m.DateTime >= s &&
                    m.DateTime < eExclusive
                )
                .OrderBy(m => m.DateTime)
                .ToListAsync();
        }
        public async Task<List<NurseShiftAssignment>> GetNurseShiftAssignmentsInRangeAsync(
    List<string> nurseIds, DateTime startDate, DateTime endDate)
        {
            var s = startDate.Date;
            var eExclusive = endDate.Date.AddDays(1);

            return await _context.NurseShiftAssignments
                .Include(a => a.Shift)               
                .Where(a =>
                    nurseIds.Contains(a.NurseId) &&
                    a.Date >= s &&
                    a.Date < eExclusive
                )
                .ToListAsync();
        }
    }
}
