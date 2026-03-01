using EA_Ecommerce.DAL.utils.SeedData;
using EA_Ecommerce.PL.utils;
using Elderly_System.BLL.Service.Classes;
using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Repositories.Classes;
using Elderly_System.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Elderly_System.PL
{
    internal static class AppConfiguration
    {
        internal static void AddConfig(this IServiceCollection services)
        {
            services.AddScoped<ISeedData, SeedData>();
            
            services.AddScoped<IAuthenticationService,AuthenticationService>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IEmailSender, EmailSetting>();
            services.AddScoped<IDonationService, DonationService>();
            services.AddScoped<IDonationRepository,DonationRepository>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IElderlySponsorRepository, ElderlySponsorRepository>();
            services.AddScoped<IElderlySponsorService, ElderlySponsorService>();
            services.AddScoped<IVistorRepository, VistorRepository>();
            services.AddScoped<IVistorService, VistorService>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<IElderlyAdminService , ElderlyAdminService>();
            services.AddScoped<IElderlyAdminRepository, ElderlyAdminRepository>();
            services.AddScoped<IElderlyNurseRepository, ElderlyNurseRepository>();
            services.AddScoped<IElderlyNurseService, ElderlyNurseService>();
            services.AddScoped<ICheckListService,CheckListService>();
            services.AddScoped<ICheckListRepository, CheckListRepository>();
            services.AddScoped<INurseShiftService , NurseShiftService>();
            services.AddScoped<INurseShiftRepository, NurseShiftRepository>();
            //services.AddScoped<>();
             






        }
    }
}
