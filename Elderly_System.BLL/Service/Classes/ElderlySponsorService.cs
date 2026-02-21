using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlySponsorService : IElderlySponsorService
    {
        private readonly IElderlySponsorRepository _repository;
        private readonly IFileService _file;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _service;

        public ElderlySponsorService(IElderlySponsorRepository repository , IFileService file , UserManager<ApplicationUser> userManager , IAuthenticationService service)
        {
            _repository = repository;
            _file = file;
            _userManager = userManager;
            _service = service;
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
                status = Status.Active
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
            var sponsorUser = await _userManager.FindByIdAsync(sponsorId);
            bool profileChanged = false;
            if (sponsorUser != null && sponsorUser.IsProfileCompleted == false)
            {
                sponsorUser.IsProfileCompleted = true;
                await _userManager.UpdateAsync(sponsorUser);
                profileChanged = true;
            }
            if (profileChanged)
            {
                var newToken = await _service.GenerateTokenAsync(sponsorId);

                return ServiceResult.SuccessWithData(new
                {
                    token = newToken,
                }, "تم إدخال بيانات المسن والطبيب ورفع التشخيص بنجاح.");
            }
            return ServiceResult.SuccessMessage("تم إدخال بيانات المسن والطبيب ورفع التشخيص بنجاح. .");
        }
        public async Task<ServiceResult> VerifyLinkAsync(VerifyElderlySponsorLinkRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.ElderlyNationalId) || string.IsNullOrWhiteSpace(req.SponsorNationalId))
                return ServiceResult.Failure("يرجى إدخال رقم هوية المسن ورقم هوية الكفيل.");

            var elderlyId = await _repository.GetElderlyIdByNationalIdAsync(req.ElderlyNationalId);
            if (elderlyId == null)
                return ServiceResult.SuccessWithData(new { isLinked = false }, "المسن غير موجود.");

            var sponsorId = await _repository.GetSponsorIdByNationalIdAsync(req.SponsorNationalId);
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.SuccessWithData(new { isLinked = false }, "الكفيل غير موجود.");

            var isLinked = await _repository.IsLinkBetweenAsync(elderlyId.Value, sponsorId);

            if (!isLinked)
                return ServiceResult.SuccessWithData(new { isLinked = false }, "لا يوجد ارتباط بين هذا الكفيل وهذا المسن.");

            return ServiceResult.SuccessWithData(new { isLinked = true }, "الارتباط صحيح.");
        }
        public async Task<ServiceResult> LinkSponsorToElderlyAsync(string sponsorId, LinkSponsorToElderlyRequest req)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("غير مصرح.");

            req.ElderlyNationalId = req.ElderlyNationalId.Trim();

            var elderlyId = await _repository.GetElderlyIdByNationalIdAsync(req.ElderlyNationalId);
            if (elderlyId == null)
                return ServiceResult.Failure("المسن غير موجود.");

            var alreadyLinked = await _repository.IsLinkBetweenAsync(elderlyId.Value, sponsorId);
            if (alreadyLinked)
                return ServiceResult.SuccessWithData(new { isLinked = true }, "أنت مرتبط بهذا المسن مسبقًا.");

            await _repository.CreateLinkAsync(elderlyId.Value, sponsorId, req.KinShip, req.Degree);
            var sponsorUser = await _userManager.FindByIdAsync(sponsorId);
            bool profileChanged = false;
            if (sponsorUser != null && sponsorUser.IsProfileCompleted == false)
            {
                sponsorUser.IsProfileCompleted = true;
                await _userManager.UpdateAsync(sponsorUser);
                profileChanged = true;

            }
            if (profileChanged)
            {
                var newToken = await _service.GenerateTokenAsync(sponsorId);

                return ServiceResult.SuccessWithData(new
                {
                    token = newToken,
                }, "تم ربط الكفيل بالمسن بنجاح");
            }
            return ServiceResult.SuccessWithData(new { isLinked = true }, "تم ربط الكفيل بالمسن بنجاح.");
        }
    }
}
