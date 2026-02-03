using EA_Ecommerce.DAL.utils.SeedData;
using EA_Ecommerce.PL.utils;
using Elderly_System.BLL.Service.Classes;
using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Elderly_System.PL
{
    internal static class AppConfiguration
    {
        internal static void AddConfig(this IServiceCollection services)
        {
            services.AddScoped<ISeedData, SeedData>();
            services.AddScoped<IAuthenticationService,AuthenticationService>();
            services.AddScoped<IEmailSender, EmailSetting>();


        }
    }
}
