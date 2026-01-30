using EA_Ecommerce.DAL.utils.SeedData;
using Elderly_System.BLL.Service.Authentication;

namespace Elderly_System.PL
{
    internal static class AppConfiguration
    {
        internal static void AddConfig(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService,AuthenticationService>();
            services.AddScoped<ISeedData, SeedData>();

        }
    }
}
