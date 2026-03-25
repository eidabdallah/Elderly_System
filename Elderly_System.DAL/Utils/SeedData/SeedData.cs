using Elderly_System.DAL.Enums;
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
                await _roleManager.CreateAsync(new IdentityRole("FirstSponsor"));
                await _roleManager.CreateAsync(new IdentityRole("SecondSponsor"));
                await _roleManager.CreateAsync(new IdentityRole("Accountant"));
                await _roleManager.CreateAsync(new IdentityRole("Cleaner"));
                await _roleManager.CreateAsync(new IdentityRole("Security"));
                await _roleManager.CreateAsync(new IdentityRole("Chef"));
                await _roleManager.CreateAsync(new IdentityRole("Secretary"));
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));

            }
            if (!await _userManager.Users.AnyAsync())
            {
                var userAdmin = new ApplicationUser()
                {
                    NationalId = "999333444",
                    Email = "bahaa@gmail.com",
                    FullName = "بهاء احمد",
                    PhoneNumber = "1234567890",
                    UserName = "BahaaBB",
                    City = "نابلس",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,
                };
                await _userManager.CreateAsync(userAdmin, "Pass@12345");
                await _userManager.AddToRoleAsync(userAdmin, "Admin");


                var cleaner1 = new Employee
                {
                    NationalId = "444555666",
                    Email = "cleaner1@gmail.com",
                    FullName = "سميحة خالد عبدالصمد",
                    PhoneNumber = "0590000004",
                    UserName = "Cleaner1",
                    City = "نابلس",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,

                    MaritalStatus = MaritalStatus.Married
                };

                var resCleaner1 = await _userManager.CreateAsync(cleaner1, "Pass@12345");
                if (resCleaner1.Succeeded)
                {
                    await _userManager.AddToRoleAsync(cleaner1, "Cleaner");
                }

                var cleaner2 = new Employee
                {
                    NationalId = "555666777",
                    Email = "cleaner2@gmail.com",
                    FullName = "هبة محمد احمد",
                    PhoneNumber = "0590000005",
                    UserName = "Cleaner2",
                    City = "نابلس",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    MaritalStatus = MaritalStatus.Single
                };

                var resCleaner2 = await _userManager.CreateAsync(cleaner2, "Pass@12345");
                if (resCleaner2.Succeeded)
                {
                    await _userManager.AddToRoleAsync(cleaner2, "Cleaner");
                }
                var cleaner3 = new Employee
                {
                    NationalId = "555666888",
                    Email = "cleaner3@gmail.com",
                    FullName = "هبة محمد احمد",
                    PhoneNumber = "0590005105",
                    UserName = "Cleaner3",
                    City = "نابلس",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    MaritalStatus = MaritalStatus.Single
                };

                var resCleaner3 = await _userManager.CreateAsync(cleaner3, "Pass@12345");
                if (resCleaner3.Succeeded)
                {
                    await _userManager.AddToRoleAsync(cleaner3, "Cleaner");
                }

                var security = new Employee
                {
                    NationalId = "333444555",
                    Email = "security@gmail.com",
                    FullName = "احمد خالد عبدالله",
                    PhoneNumber = "0590000003",
                    UserName = "Security1",
                    City = "نابلس",
                    Gender = Gender.Male,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    MaritalStatus = MaritalStatus.Married
                };

                var resSecurity = await _userManager.CreateAsync(security, "Pass@12345");
                if (resSecurity.Succeeded)
                {
                    await _userManager.AddToRoleAsync(security, "Security");
                }

                var accountant = new Employee
                {
                    NationalId = "111222333",
                    Email = "accountant@gmail.com",
                    FullName = "محمد خالد احمد",
                    PhoneNumber = "0590000001",
                    UserName = "Accountant1",
                    City = "نابلس",
                    Gender = Gender.Male,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    EducationLevel = EducationLevel.University,
                    FieldOfStudy = "محاسبة",
                    YearsOfStudy = 4,
                    YearOfGraduation = "2020",
                    MaritalStatus = MaritalStatus.Married,
                };
                var res = await _userManager.CreateAsync(accountant, "Pass@12345");
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(accountant, "Accountant");
                    _context.WorkExperiences.AddRange(
                        new WorkExperience
                        {
                            WorkName = "شركة عسل",
                            WorkLocation = "نابلس",
                            JobTitle = "محاسب",
                            EmployeeId = accountant.Id
                        },
                        new WorkExperience
                        {
                            WorkName = "شركة فوتهيل",
                            WorkLocation = "رام الله",
                            JobTitle = "محاسب مساعد",
                            EmployeeId = accountant.Id
                        }
                    );
                }

                var chef = new Employee
                {
                    NationalId = "222333444",
                    Email = "chef@gmail.com",
                    FullName = "سمر أحمد علي",
                    PhoneNumber = "0590000002",
                    UserName = "Chef1",
                    City = "نابلس",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    EducationLevel = EducationLevel.Secondary,
                    FieldOfStudy = "فنون الطبخ",
                    YearsOfStudy = 2,
                    YearOfGraduation = "2018",
                    MaritalStatus = MaritalStatus.Married,
                };
                var resChef = await _userManager.CreateAsync(chef, "Pass@12345");
                if (resChef.Succeeded)
                {
                    await _userManager.AddToRoleAsync(chef, "Chef");

                    _context.WorkExperiences.AddRange(
                        new WorkExperience
                        {
                            WorkName = "مطعم الياسمين",
                            WorkLocation = "نابلس",
                            JobTitle = "طباخة",
                            EmployeeId = chef.Id
                        },
                        new WorkExperience
                        {
                            WorkName = "مطبخ البيت السعيد",
                            WorkLocation = "رام الله",
                            JobTitle = "مساعدة طباخة",
                            EmployeeId = chef.Id
                        }
                    );
                }
                var Secretary = new Employee
                {
                    NationalId = "444555662",
                    Email = "Secretary@gmail.com",
                    FullName = "اسماء احمد علي",
                    PhoneNumber = "0590000013",
                    UserName = "Secretary2",
                    City = "نابلس",
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    EducationLevel = EducationLevel.Secondary,
                    FieldOfStudy = "سكرتيرة",
                    YearsOfStudy = 2,
                    YearOfGraduation = "2018",
                    MaritalStatus = MaritalStatus.Married,
                };
                var resSecretary = await _userManager.CreateAsync(Secretary, "Pass@12345");
                if (resSecretary.Succeeded)
                {
                    await _userManager.AddToRoleAsync(Secretary, "Secretary");

                    _context.WorkExperiences.AddRange(
                        new WorkExperience
                        {
                            WorkName = "جامعة النجاح",
                            WorkLocation = "نابلس",
                            JobTitle = "سكرتيرة",
                            EmployeeId = Secretary.Id
                        }
                    );
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task SeedShiftsAsync()
        {
            if (await _context.Shifts.AnyAsync())
                return;
            _context.Shifts.AddRange(
                new Shift { ShiftKey = 'A', StartTime = new TimeSpan(3, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Shift { ShiftKey = 'B', StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(17, 0, 0) },
                new Shift { ShiftKey = 'C', StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(0, 0, 0) }
            );
            await _context.SaveChangesAsync();

        }
    }
}
