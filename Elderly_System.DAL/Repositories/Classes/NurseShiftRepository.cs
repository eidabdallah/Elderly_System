using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class NurseShiftRepository : INurseShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public NurseShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Nurse>> GetActiveNursesAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .OfType<Nurse>()
                .Where(n => n.Status == Status.Active)
                .OrderBy(n => n.FullName)
                .ToListAsync();
        }

        public async Task<List<Nurse>> GetActiveNursesByIdsAsync(List<string> nurseIds)
        {
            return await _context.Users
                .AsNoTracking()
                .OfType<Nurse>()
                .Where(n => n.Status == Status.Active && nurseIds.Contains(n.Id))
                .ToListAsync();
        }

        public async Task<List<NurseShiftAssignment>> GetAssignmentsByDateAndNurseIdsAsync(DateTime date, List<string> nurseIds)
        {
            var d = date.Date;

            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Where(a => a.Date.Date == d && nurseIds.Contains(a.NurseId))
                .ToListAsync();
        }

        public async Task<Dictionary<char, Shift>> GetShiftsByKeysAsync(List<char> keys)
        {
            var upperKeys = keys.Select(char.ToUpperInvariant).Distinct().ToList();

            var shifts = await _context.Shifts
                .AsNoTracking()
                .Where(s => upperKeys.Contains(s.ShiftKey))
                .ToListAsync();

            return shifts.ToDictionary(s => s.ShiftKey, s => s);
        }

        public async Task AddAssignmentsAsync(List<NurseShiftAssignment> assignments)
        {
            await _context.Set<NurseShiftAssignment>().AddRangeAsync(assignments);
        }
        public async Task<List<NurseShiftAssignment>> GetAssignmentsInRangeAsync(DateTime start, DateTime end)
        {
            var s = start.Date;
            var e = end.Date;

            var endExclusive = e.AddDays(1);

            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Shift) 
                .Where(a => a.Date >= s && a.Date < endExclusive)
                .ToListAsync();
        }
        public async Task<Nurse?> GetActiveNurseByIdAsync(string nurseId)
        {
            return await _context.Users
                .AsNoTracking()
                .OfType<Nurse>()
                .FirstOrDefaultAsync(n => n.Id == nurseId && n.Status == Status.Active);
        }

        public async Task<List<NurseShiftAssignment>> GetAssignmentsInRangeByNurseAsync(string nurseId, DateTime start, DateTime end)
        {
            var s = start.Date;
            var e = end.Date.AddDays(1); 

            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Include(a => a.Shift) 
                .Where(a => a.NurseId == nurseId && a.Date >= s && a.Date < e)
                .ToListAsync();
        }

        public async Task<List<DateTime>> GetScheduledDaysInRangeAsync(DateTime start, DateTime end)
        {
            var s = start.Date;
            var e = end.Date.AddDays(1);

            return await _context.Set<NurseShiftAssignment>()
                .AsNoTracking()
                .Where(a => a.Date >= s && a.Date < e)
                .Select(a => a.Date.Date)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
