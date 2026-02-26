using Elderly_System.DAL.DTO.Response.Statistics;
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
        public async Task<int> CountSponsorAsync()
        {
            return await _context.Sponsors.CountAsync();
        }

        public async Task<int> CountDonationsAsync()
        {
            return await _context.Donations.CountAsync();
        }

        public async Task<int> CountActivitiesAsync()
        {
            return await _context.Activities.CountAsync();
        }

        public async Task<int> CountRoomsAsync()
        {
            return await _context.Rooms.CountAsync();
        }

        public async Task<List<DonationMonthDto>> GetDonationsOverTimeAsync()
        {
            var currentYear = DateTime.Now.Year;

            var donations = await _context.Donations
                .Where(d => d.DonationDate.Year == currentYear)
                .GroupBy(d => d.DonationDate.Month)
                .Select(g => new
                {
                    MonthNumber = g.Key,
                    Donations = g.Count()
                })
                .ToListAsync();

            var arabicMonths = new[]
            {
        "يناير",
        "فبراير",
        "مارس",
        "أبريل",
        "مايو",
        "يونيو",
        "يوليو",
        "أغسطس",
        "سبتمبر",
        "أكتوبر",
        "نوفمبر",
        "ديسمبر"
    };

            var result = Enumerable.Range(1, 12)
                .Select(month => new DonationMonthDto
                {
                    Month = arabicMonths[month - 1],
                    Donations = donations.FirstOrDefault(d => d.MonthNumber == month)?.Donations ?? 0
                })
                .ToList();

            return result;
        }
    }
}
