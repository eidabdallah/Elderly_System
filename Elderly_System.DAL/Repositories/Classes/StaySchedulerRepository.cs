using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class StaySchedulerRepository : IStaySchedulerRepository
    {
        private readonly ApplicationDbContext _context;

        public StaySchedulerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ResidentStay>> GetStaysToAutoFinishAsync(DateTime today)
        {
            return await _context.ResidentStays
                .Include(s => s.Room)
                .Where(s => s.Status == Status.Active
                    && s.EndDate != null
                    && s.EndDate.Value.Date < today.Date)
                .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
