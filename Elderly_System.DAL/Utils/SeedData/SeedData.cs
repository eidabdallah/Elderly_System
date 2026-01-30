using EderlySystem.DAL.Enums;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EA_Ecommerce.DAL.utils.SeedData
{
    public class SeedData : ISeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task IdentityDataSeedingAsync()
        { 
            if(!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin")); 
                await _roleManager.CreateAsync(new IdentityRole("Nurse"));
                await _roleManager.CreateAsync(new IdentityRole("Sponsor"));
                await _roleManager.CreateAsync(new IdentityRole("Secretary"));
            }
            if (!await _userManager.Users.AnyAsync())
            {
                var userAdmin = new ApplicationUser()
                {
                    NationalId = "999333444",
                    Email = "bahaa@gmail.com",
                    FullName = "bahaa bbb",
                    PhoneNumber = "1234567890",
                    UserName = "BahaaBB",
                    City = "Nablus",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,
                };

                await _userManager.CreateAsync(userAdmin, "Pass@12345");
                await _userManager.AddToRoleAsync(userAdmin, "Admin");

            }

            await _context.SaveChangesAsync();
        }
        public async Task SeedShiftsAsync()
        {
            if (await _context.Shifts.AnyAsync())
                return;
            _context.Shifts.AddRange(
                new Shift { ShiftKey = 'A', StartTime = new TimeSpan(2, 0, 0), EndTime = new TimeSpan(9, 0, 0) },
                new Shift { ShiftKey = 'B', StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(16, 0, 0) },
                new Shift { ShiftKey = 'C', StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(22, 0, 0) }
            );
        }
    }
}
