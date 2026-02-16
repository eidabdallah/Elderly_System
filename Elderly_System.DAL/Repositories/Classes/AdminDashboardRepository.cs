using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminDashboardRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<int> CountElderliesAsync()
        {
            return await _context.Elderlies.CountAsync();
        }

        public async Task<int> CountUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Count;
        }

        public async Task<int> CountDonationsToDateAsync(DateTime today)
        {
            var tomorrow = today.Date.AddDays(1);
            return await _context.Donations
                .CountAsync(d => d.DonationDate < tomorrow);
        }

        public async Task<int> CountEventsToDateAsync(DateTime today)
        {
            var tomorrow = today.Date.AddDays(1);
            return await _context.Activities
                .CountAsync(e => e.Date < tomorrow);
        }
    }
}
