using CloudinaryDotNet;
using ElderlySystem.BLL.Configurations;
using ElderlySystem.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace Elderly_System.PL
{
    public class Program
    {
        public static void Main(string[] args)
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

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

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
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
