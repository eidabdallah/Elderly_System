using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.DTO.Request.Auth;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Elderly_System.BLL.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
           , IConfiguration configuration, IEmailSender emailSender){
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        public async Task<ServiceResult> RegisterAsync(RegisterRequest request)
        {
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail is not null)
                return ServiceResult.Failure("البريد الإلكتروني مستخدم بالفعل.");

            var existingPhoneNumber = await _userManager.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingPhoneNumber)
                return ServiceResult.Failure("رقم الهاتف مستخدم بالفعل.");

            var nationalIdExists = await _userManager.Users.AnyAsync(u => u.NationalId == request.NationalId);
            if (nationalIdExists)
                return ServiceResult.Failure("رقم الهوية مستخدم بالفعل.");

            var user = new Sponsor
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate,
                City = request.City,
                Street = request.Street,
                NationalId = request.NationalId,
                Note = string.IsNullOrWhiteSpace(request.Note) ? "لا يوجد" : request.Note,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Select(e => e.Description).FirstOrDefault() ?? "فشل إنشاء حساب.";
                return ServiceResult.Failure(error);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Sponsor");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return ServiceResult.Failure("حدث خطأ أثناء تعيين الصلاحيات ، لم يتم تسجيل الحساب يرجى المحاولة مرة اخرى.");
            }
            return ServiceResult.SuccessMessage("تم تسجيل الحساب بنجاح.");
        }
    }
}
