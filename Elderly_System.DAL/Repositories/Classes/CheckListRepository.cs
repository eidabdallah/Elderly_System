using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class CheckListRepository : ICheckListRepository
    {
        private readonly ApplicationDbContext _context;

        public CheckListRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Elderly?> GetElderlyByIdAsync(int elderlyId)
        {
            return await _context.Elderlies
                .FirstOrDefaultAsync(x => x.Id == elderlyId);
        }

        public async Task<Nurse?> GetNurseByIdAsync(string nurseId)
        {
            return await _context.Nurses
                .FirstOrDefaultAsync(x => x.Id == nurseId);
        }

        public async Task AddCheckListAsync(CheckList checkList)
        {
            await _context.CheckLists.AddAsync(checkList);
        }

        public async Task<CheckList?> GetCheckListByIdAsync(int checkListId)
        {
            return await _context.CheckLists
                .Include(x => x.Elderly)
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.Id == checkListId);
        }

        public async Task<List<CheckListListResponse>> GetCheckListsByElderlyIdAsync(int elderlyId)
        {
            return await _context.CheckLists
                .AsNoTracking()
                .Where(x => x.ElderlyId == elderlyId)
                .OrderByDescending(x => x.DateTime)
                .Select(x => new CheckListListResponse
                {
                    CheckListId = x.Id,
                    Date = x.DateTime.ToString("MM-dd-yyyy"),
                    Temperature = x.Temperature ?? "-",
                    Pulse = x.Pulse ?? "-",
                    BloodSugar = x.BloodSugar ?? "-",
                    BloodPressure = x.BloodPressure ?? "-",
                    Intake = x.Intake ?? "-",
                    Output = x.Output ?? "-",
                })
                .ToListAsync();
        }
        public async Task<string?> GetNurseShiftKeyByDateAsync(string nurseId, DateTime date)
        {
            var d = date.Date;

            return await _context.Set<NurseShiftAssignment>()
               .AsNoTracking()
               .Where(x => x.NurseId == nurseId && x.Date.Date == d)
               .Select(x => x.Shift.ShiftKey.ToString())
               .FirstOrDefaultAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task DeleteCheckListAsync(CheckList checkList)
        {
            _context.CheckLists.Remove(checkList);
            return Task.CompletedTask;
        }

    }
}
