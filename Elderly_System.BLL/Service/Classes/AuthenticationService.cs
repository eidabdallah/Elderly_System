using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Auth;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Data;
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
        private readonly IAuthenticationRepository _repository;
        private readonly ApplicationDbContext _dbContext;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
           , IConfiguration configuration, IEmailSender emailSender , IFileService file , IAuthenticationRepository repository , ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _file = file;
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> RegisterStaffAsync(RegisterStaffRequest request, HttpRequest httpRequest)
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

            if (request.Certificate is null)
                return ServiceResult.Failure("الشهادة مطلوبة.");

            var allowed = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            if (!HasAllowedExt(request.Certificate, allowed))
                return ServiceResult.Failure("الشهادة يجب أن تكون صورة أو ملف PDF.");

            var uploaded = await _file.UploadAsync(request.Certificate, "certificates");

            var nurse = new Nurse
            {
                ImageCertificate = uploaded.Url,
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                NationalId = request.NationalId,
                Gender = request.Gender,
                JobTitle = "ممرض",
                EducationLevel = request.EducationLevel,
                MaritalStatus = request.MaritalStatus,
                FieldOfStudy = request.FieldOfStudy?.Trim(),
                YearsOfStudy = request.YearsOfStudy,
                YearOfGraduation = request.YearOfGraduation?.Trim(),
                WorkExperiences = new List<WorkExperience>()
            };

            if (request.WorkExperiences != null && request.WorkExperiences.Count > 0)
            {
                foreach (var we in request.WorkExperiences)
                {
                    if (string.IsNullOrWhiteSpace(we.WorkName) || string.IsNullOrWhiteSpace(we.JobTitle))
                        return ServiceResult.Failure("كل خبرة لازم يكون فيها مكان العمل و الدور الوظيفي");

                    nurse.WorkExperiences.Add(new WorkExperience
                    {
                        WorkName = we.WorkName.Trim(),
                        WorkLocation = we.WorkLocation.Trim(),
                        JobTitle = we.JobTitle.Trim(),
                    });
                }
            }
            var create = await _userManager.CreateAsync(nurse, request.Password);
            if (!create.Succeeded)
                return ServiceResult.Failure(string.Join(" | ", create.Errors.Select(e => e.Description)));

            var addRole = await _userManager.AddToRoleAsync(nurse, Role.Nurse.ToString());
            if (!addRole.Succeeded)
                return ServiceResult.Failure("تم إنشاء المستخدم لكن فشل تعيين الدور.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(nurse);
            var tokenEncoded = Uri.EscapeDataString(token);
            var emailUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/Identity/Account/ConfirmEmail?token={tokenEncoded}&userId={nurse.Id}";

            await _emailSender.SendEmailAsync(nurse.Email!, "تأكيد البريد الالكتروني",
                $"<h1>أهلاا {nurse.FullName} ❤️</h1><a href='{emailUrl}'>تأكيد</a>");

            return ServiceResult.SuccessMessage("تم تسجيل الحساب بنجاح، يرجى تأكيد البريد الإلكتروني.");
        }
        public async Task<ServiceResult> RegisterAsync(RegisterRequest request, HttpRequest httpRequest)
        {
            if (request.SponsorDegree == SponsorDegree.Second)
            {
                if (string.IsNullOrWhiteSpace(request.NationalIdElderly))
                    return ServiceResult.Failure("رقم هوية المسن مطلوب.");

                if (string.IsNullOrWhiteSpace(request.KinShip) || string.IsNullOrWhiteSpace(request.Degree))
                    return ServiceResult.Failure("صلة القرابة والدرجة مطلوبة.");

                var elderlyCheck = await _repository.GetActiveElderlyByNationalIdAsync(request.NationalIdElderly);
                if (elderlyCheck == null)
                    return ServiceResult.Failure("المسن غير موجود في النظام حاليا او لم يتم تفعيل حالته بعد.");
            }

            if (request.SponsorDegree == SponsorDegree.First)
            {
                if (string.IsNullOrWhiteSpace(request.NationalIdElderly))
                    return ServiceResult.Failure("رقم هوية المسن مطلوب.");

                if (request.MaritalStatus is null || request.BDate is null || request.ReportDate is null)
                    return ServiceResult.Failure("يرجى إدخال الحالة الاجتماعية وتاريخ الميلاد وتاريخ التقرير.");

                if (request.NationalIdImage is null || request.HealthInsurance is null || request.DiagnosisFile is null)
                    return ServiceResult.Failure("يرجى إرفاق جميع الملفات المطلوبة.");
            }

            await using var tx = await  _dbContext.Database.BeginTransactionAsync();
            try
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
                    Gender = request.GenderSponsor,
                    UserName = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    City = request.City,
                    NationalId = request.NationalId,
                    Degree = request.SponsorDegree,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    return ServiceResult.Failure("فشل في انشاء الحساب");

                var roleResult = await _userManager.AddToRoleAsync(user, "Sponsor");
                if (!roleResult.Succeeded)
                    return ServiceResult.Failure("فشل في إضافة صلاحية المستخدم.");

                if (request.SponsorDegree == SponsorDegree.First)
                {
                    var exists = await _repository.IsElderlyNationalIdExistsAsync(request.NationalIdElderly);
                    if (exists)
                        return ServiceResult.Failure("رقم هوية المسن مستخدم مسبقاً.");

                    var allowed = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

                    if (!HasAllowedExt(request.DiagnosisFile!, allowed) ||
                        !HasAllowedExt(request.NationalIdImage!, allowed) ||
                        !HasAllowedExt(request.HealthInsurance!, allowed))
                    {
                        return ServiceResult.Failure("يجب أن تكون جميع الملفات صورًا أو ملفات PDF.");
                    }

                    var idImg = await _file.UploadAsync(request.NationalIdImage!, "elderly/nationalid");
                    var insurance = await _file.UploadAsync(request.HealthInsurance!, "elderly/insurance");
                    var diagnosis = await _file.UploadAsync(request.DiagnosisFile!, "elderly/diagnosis");

                    var elderly = new Elderly
                    {
                        Name = request.Name!,
                        NationalId = request.NationalIdElderly!,
                        Doctrine = request.Doctrine!,
                        MaritalStatus = request.MaritalStatus!.Value,
                        City = request.CityElderly!,
                        Street = request.Street!,
                        HealthStatus = request.HealthStatus!,
                        Diseases = request.Diseases ?? new List<string>(),
                        BDate = request.BDate!.Value,
                        ReasonRegister = request.ReasonRegister!,
                        NationalIdImage = idImg.Url,
                        HealthInsurance = insurance.Url,
                        status = Status.InActive
                    };

                    var doctor = new Doctor
                    {
                        Name = request.DoctorName!,
                        WorkPlace = request.WorkPlace!,
                        Phone = request.DoctorPhone!
                    };

                    var report = new MedicalReport
                    {
                        Date = request.ReportDate!.Value,
                        DiagnosisUrl = diagnosis.Url,
                        DiagnosisPublicId = diagnosis.PublicId
                    };

                    var link = new ElderlySponsor
                    {
                        SponsorId = user.Id,
                        KinShip = request.KinShip!,
                        Degree = request.Degree!
                    };

                    await _repository.AddAsync(elderly, doctor, report, link);
                }
                else if (request.SponsorDegree == SponsorDegree.Second)
                {
                    if (string.IsNullOrWhiteSpace(request.NationalIdElderly))
                        return ServiceResult.Failure("رقم هوية المسن مطلوب.");

                    if (string.IsNullOrWhiteSpace(request.KinShip) || string.IsNullOrWhiteSpace(request.Degree))
                        return ServiceResult.Failure("صلة القرابة والدرجة مطلوبة.");

                    var elderly = await _repository.GetActiveElderlyByNationalIdAsync(request.NationalIdElderly!);
                    if (elderly == null)
                        return ServiceResult.Failure("المسن غير موجود في النظام حاليا او لم يتم تفعيل حالته بعد.");

                    var alreadyLinked = await _repository.IsSponsorLinkedToElderlyAsync(elderly.Id, user.Id);
                    if (alreadyLinked)
                        return ServiceResult.Failure("هذا الكفيل مرتبط مسبقاً بهذا المسن.");

                    var link = new ElderlySponsor
                    {
                        SponsorId = user.Id,
                        ElderlyId = elderly.Id,
                        KinShip = request.KinShip!,
                        Degree = request.Degree!
                    };

                    await _repository.AddElderlySponsorLinkAsync(link);
                }
                await tx.CommitAsync();

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var tokenEncoded = Uri.EscapeDataString(token);
                var emailUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/Identity/Account/ConfirmEmail?token={tokenEncoded}&userId={user.Id}";

                await _emailSender.SendEmailAsync(user.Email!, "تأكيد البريد الالكتروني",
                    $"<h1>أهلاااا {user.FullName} ❤️</h1><a href='{emailUrl}'>تأكيد</a>");

                return ServiceResult.SuccessMessage("تم تسجيل الحساب بنجاح، يرجى تأكيد البريد الإلكتروني.");
            }
            catch
            {
                await tx.RollbackAsync();
                return ServiceResult.Failure("حدث خطأ أثناء التسجيل، لم يتم حفظ أي بيانات.");
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
                return ServiceResult.SuccessWithData(Token,"تم تسجيل الدخول بنجاح.");
            }
            else if (result.IsNotAllowed) return ServiceResult.Failure("يرجى تأكيد البريد الإلكتروني أولاً.");
            else return ServiceResult.Failure("خطا في الايميل او كلمة السر");
        }
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
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
                PhoneNumber = user.PhoneNumber!,
                city = user.City,
                NationalId = user.NationalId,
                Gender = user.Gender
            };
            return ServiceResult.SuccessWithData(response, "تم جلب معلومات المستخدم");
        }
        private static bool HasAllowedExt(IFormFile file, params string[] allowed)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowed.Contains(ext);
        }
    }
}
