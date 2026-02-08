using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Auth;
using Elderly_System.DAL.Enums;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.DTO.Request.Auth;
using ElderlySystem.DAL.DTO.Response.User;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Elderly_System.BLL.Service.Classes
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IFileService _file;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
           , IConfiguration configuration, IEmailSender emailSender , IFileService file)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _file = file;
        }

        public async Task<ServiceResult> RegisterStaffAsync(RegisterStaffRequest request , HttpRequest HttpRequest)
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
            var uploaded = await _file.UploadAsync(request.Certificate!, "certificates");

            var nurse = new Nurse
            {
                ImageCertificate = uploaded.Url,

                JobTitle = request.JobTitle,
                HireDate = request.HireDate,
                EducationLevel = request.EducationLevel,
                MaritalStatus = request.MaritalStatus,
                FieldOfStudy = request.FieldOfStudy,
                YearsOfStudy = request.YearsOfStudy,
                AcademicDegree = request.AcademicDegree,
                YearOfGraduation = request.YearOfGraduation,
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                NationalId = request.NationalId,
                Gender = request.Gender,
                Status = Status.Pending
            };
            var create = await _userManager.CreateAsync(nurse, request.Password);
            if (!create.Succeeded)
                return ServiceResult.Failure(string.Join(" | ", create.Errors.Select(e => e.Description)));

            var addRole = await _userManager.AddToRoleAsync(nurse, Role.Nurse.ToString());
            if (!addRole.Succeeded)
                return ServiceResult.Failure("تم إنشاء المستخدم لكن فشل تعيين الدور.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(nurse);
            var tokenEncoded = Uri.EscapeDataString(token);
            var emailUrl = $"{HttpRequest.Scheme}://{HttpRequest.Host}/api/Identity/Account/ConfirmEmail?token={tokenEncoded}&userId={nurse.Id}";
            await _emailSender.SendEmailAsync(nurse.Email!, "تأكيد البريد الالكتروني",
              $"<h1>Hello {nurse.FullName} ❤️</h1><a href='{emailUrl}'>تأكيد</a>");

            return ServiceResult.SuccessMessage("تم تسجيل الحساب بنجاح، يرجى تأكيد البريد الإلكتروني.");
        }
        public async Task<ServiceResult> RegisterAsync(RegisterRequest request, HttpRequest HttpRequest)
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
                Gender = request.Gender,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                NationalId = request.NationalId,
                Note = string.IsNullOrWhiteSpace(request.Note) ? "لا يوجد" : request.Note,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var tokenEncoded = Uri.EscapeDataString(token);
                var tokenEncoded = WebUtility.UrlEncode(token);
                var emailUrl = $"{HttpRequest.Scheme}://{HttpRequest.Host}/api/Identity/Account/ConfirmEmail?token={tokenEncoded}&userId={user.Id}";
                await _userManager.AddToRoleAsync(user, "Sponsor");
                await _emailSender.SendEmailAsync(user.Email!, "تأكيد البريد الالكتروني",
                  $"<h1>أهلاااا {user.FullName} ❤️</h1><a href='{emailUrl}'>تأكيد</a>");
                return ServiceResult.SuccessMessage("تم تسجيل الحساب بنجاح، يرجى تأكيد البريد الإلكتروني.");
            }
            else
            {
                return ServiceResult.Failure("فشل في انشاء الحساب");
            }
        }
        public async Task<string> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return "المستخدم غير موجود";
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return "email confirmed successfully";
            }
            return "email confirmation failed";
        }

        public async Task<ServiceResult> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ServiceResult.Failure("خطأ في الايميل أو كلمة السر");
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (result.Succeeded)
            {
                if (user.Status == Status.Pending) return ServiceResult.SuccessMessage("تم تسجيل الدخول بنجاح ، لكن حسابك لم يتم القبول عليه بعد");
                if (user.Status == Status.InActive) return ServiceResult.SuccessMessage("تم تسجيل الدخول بنجاح ، لكن حسابك مقفول من الادمن");
                var Token = await CreateTokenAsync(user);
                return ServiceResult.SuccessWithData(Token, "تم تسجيل الدخول بنجاح.");
            }
            else if (result.IsLockedOut) return ServiceResult.Failure("تم قفل الحساب ، يرجى التواصل مع الادارة للاستفسار.");
            else if (result.IsNotAllowed) return ServiceResult.Failure("يرجى تأكيد البريد الإلكتروني أولاً.");
            else return ServiceResult.Failure("خطا في الايميل او كلمة السر");

        }
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwtOptions")["SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ServiceResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ServiceResult.Failure("المستخدم غير موجود.");

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(
                request.Email,
                "إعادة تعيين كلمة المرور",
                $@"
            <div style='font-family:Arial,Helvetica,sans-serif; direction:rtl; text-align:right; background:#f4f6f8; padding:20px; border-radius:10px;'>
                <h2 style='color:#007bff;'>طلب إعادة تعيين كلمة المرور</h2>
                <p>مرحبًا <strong>{user.FullName}</strong>،</p>
                <p>تم استلام طلب لإعادة تعيين كلمة المرور الخاصة بك.</p>
                <p>رمز التحقق الخاص بك هو:</p>
                <h1 style='color:#28a745; text-align:center;'>{code}</h1>
                <p>سيكون هذا الرمز صالحًا لمدة <strong>15 دقيقة</strong>.</p>
                <p>إذا لم تقم بطلب إعادة تعيين كلمة المرور، يمكنك تجاهل هذه الرسالة.</p>
                <br/>
                <p style='color:#6c757d;'>مع تحيات فريق الدعم 👋</p>
            </div>"
            );
            return ServiceResult.SuccessMessage("تم إرسال رمز إعادة تعيين كلمة المرور إلى بريدك الإلكتروني.");
        }

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ServiceResult.Failure("المستخدم غير موجود.");
            if (user.CodeResetPassword != request.Code)
                return ServiceResult.Failure("رمز التحقق غير صحيح.");

            if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
                return ServiceResult.Failure("انتهت صلاحية رمز التحقق، الرجاء طلب رمز جديد.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
                return ServiceResult.Failure("حدث خطأ أثناء تغيير كلمة المرور.");

            user.CodeResetPassword = null;
            user.PasswordResetCodeExpiry = null;
            await _userManager.UpdateAsync(user);


            return ServiceResult.SuccessMessage("تم تغيير كلمة المرور بنجاح.");
        }

        public async Task<ServiceResult> AuthMeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return ServiceResult.Failure("المستخدم غير موجود.");


            var response = new AuthMeResponse
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber!
            };
            return ServiceResult.SuccessWithData(response, "تم جلب معلومات المستخدم");
        }
    }
}
