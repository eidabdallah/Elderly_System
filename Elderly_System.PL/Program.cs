using CloudinaryDotNet;
using EA_Ecommerce.DAL.utils.SeedData;
using ElderlySystem.BLL.Configurations;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json;


namespace Elderly_System.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
            builder.Services.AddConfig();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
            builder.Services.AddSingleton<Cloudinary>(sp =>
            {
                var s = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new CloudinaryDotNet.Account(s.CloudName, s.ApiKey, s.ApiSecret);
                return new Cloudinary(account);
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            var userPolicy = "";
            builder.Services.AddCors(options => {
                options.AddPolicy(name: userPolicy, policy =>
                {
                    policy.AllowAnyOrigin();
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("jwtOptions")["SecretKey"]!))
                };
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            var scope = app.Services.CreateScope();
            var objectOfSeedData = scope.ServiceProvider.GetRequiredService<ISeedData>();
            await objectOfSeedData.IdentityDataSeedingAsync();
            await objectOfSeedData.SeedShiftsAsync();



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";

                var message = response.StatusCode switch
                {
                    401 => "غير مصرح. الرجاء تسجيل الدخول مرة أخرى.",
                    403 => "ليس لديك صلاحية للوصول.",
                    404 => "المسار غير موجود.",
                    _ => "حدث خطأ غير متوقع."
                };

                var payload = JsonSerializer.Serialize(new { message, status = response.StatusCode });
                await response.WriteAsync(payload);
            });
            app.UseAuthentication();
            app.UseCors(userPolicy);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
