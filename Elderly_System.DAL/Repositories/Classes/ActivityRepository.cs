using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddActivityAsync(Activity activity)
        {
            await _context.AddAsync(activity);
            await _context.SaveChangesAsync();
        }
        public async Task<Activity?> GetActivityByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.ActivityOrganizations)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Activity>> GetAllActivitiesAsync()
        {
            return await _context.Activities
                .Include(a => a.ActivityOrganizations)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
    }
}
