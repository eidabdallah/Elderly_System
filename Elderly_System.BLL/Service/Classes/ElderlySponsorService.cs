using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlySponsorService : IElderlySponsorService
    {
        private readonly IElderlySponsorRepository _repository;
        private readonly IFileService _file;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ElderlySponsorService(IElderlySponsorRepository repository , IFileService file , UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _repository = repository;
            _file = file;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        private static bool HasAllowedExt(IFormFile file, params string[] allowed)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowed.Contains(ext);
        }

        public async Task<ServiceResult> AddElderlyWithDoctorAsync(string sponsorId, AddElderlyWithDoctorRequest request)
        {
            var exists = await _repository.IsElderlyNationalIdExistsAsync(request.NationalId);
            if (exists)
                return ServiceResult.Failure("رقم هوية المسن مستخدم مسبقاً.");

            var allowed = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

            if (!HasAllowedExt(request.DiagnosisFile, allowed) || !HasAllowedExt(request.NationalIdImage, allowed) ||
                !HasAllowedExt(request.HealthInsurance, allowed)){
                return ServiceResult.Failure("يجب أن تكون جميع الملفات صورًا أو ملفات PDF.");
            }

            var idImg = await _file.UploadAsync(request.NationalIdImage, "elderly/nationalid");
            var insurance = await _file.UploadAsync(request.HealthInsurance, "elderly/insurance");
            var diagnosis = await _file.UploadAsync(request.DiagnosisFile, "elderly/diagnosis");

            var elderly = new Elderly
            {
                Name = request.Name,
                NationalId = request.NationalId,
                Doctrine = request.Doctrine,
                MaritalStatus = request.MaritalStatus,
                City = request.City,
                Street = request.Street,
                HealthStatus = request.HealthStatus,
                Diseases = request.Diseases ?? new List<string>(),
                BDate = request.BDate,
                ReasonRegister = request.ReasonRegister,
                NationalIdImage = idImg.Url,
                HealthInsurance = insurance.Url,
                status = Status.Pending
            };
            var doctor = new Doctor
            {
                Name = request.DoctorName,
                WorkPlace = request.WorkPlace,
                Phone = request.DoctorPhone
            };
            var report = new MedicalReport
            {
                Date = request.ReportDate,
                DiagnosisUrl = diagnosis.Url,
                DiagnosisPublicId = diagnosis.PublicId
            };
            var link = new ElderlySponsor
            {
                SponsorId = sponsorId,
                KinShip = request.KinShip,
                Degree = request.Degree
            };
            await _repository.AddAsync(elderly, doctor, report, link);
            return ServiceResult.SuccessMessage("تم إدخال بيانات المسن والطبيب ورفع التشخيص بنجاح. .");
        }
        public async Task<ServiceResult> RegisterCoSponsorAsync(string currentSponsorId, RegisterCoSponsorRequest request, HttpRequest httpRequest)
        {
            var elderlyId = await _repository.GetElderlyIdForSponsorAsync(currentSponsorId);
            if (elderlyId is null)
                return ServiceResult.Failure("لا يوجد مسن مرتبط بهذا الكفيل.");

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail is not null)
                return ServiceResult.Failure("البريد الإلكتروني مستخدم بالفعل.");

            var existingPhoneNumber = await _userManager.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingPhoneNumber)
                return ServiceResult.Failure("رقم الهاتف مستخدم بالفعل.");

            var nationalIdExists = await _userManager.Users.AnyAsync(u => u.NationalId == request.NationalId);
            if (nationalIdExists)
                return ServiceResult.Failure("رقم الهوية مستخدم بالفعل.");
            var userNameExists = await _userManager.Users.AnyAsync(u => u.UserName == request.UserName);
            if (userNameExists)
                return ServiceResult.Failure("اسم المستخدم مستخدم بالفعل.");

            var newSponsor = new Sponsor
            {
                FullName = request.FullName,
                Status = Status.Active,
                Email = request.Email,
                UserName = request.UserName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                NationalId = request.NationalId,
                Note = string.IsNullOrWhiteSpace(request.Note) ? "لا يوجد" : request.Note
            };

            var create = await _userManager.CreateAsync(newSponsor, request.Password);
             if (!create.Succeeded)
                 return ServiceResult.Failure("فشل في إنشاء الحساب.");
            await _userManager.AddToRoleAsync(newSponsor, "Sponsor");

            var linkExists = await _repository.LinkExistsAsync(elderlyId.Value, newSponsor.Id);
            if (!linkExists)
            {
                await _repository.AddLinkAsync(
                    elderlyId.Value,
                    newSponsor.Id,
                    request.KinShip,
                    request.Degree);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newSponsor);
            var tokenEncoded = Uri.EscapeDataString(token);
            var emailUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/Identity/Account/ConfirmEmail?token={tokenEncoded}&userId={newSponsor.Id}";

            await _emailSender.SendEmailAsync(newSponsor.Email!, "تأكيد البريد الالكتروني",
                $"<h1>Hello {newSponsor.UserName} ❤️</h1><a href='{emailUrl}'>تأكيد</a>");

            return ServiceResult.SuccessMessage("تم تسجيل الكفيل وربطه بالمسن بنجاح، يرجى تأكيد البريد الإلكتروني.");
        }
    }
}
