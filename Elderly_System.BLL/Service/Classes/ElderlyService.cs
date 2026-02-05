using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Http;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlyService : IElderlyService
    {
        private readonly IElderlyRepository _repository;
        private readonly IFileService _file;

        public ElderlyService(IElderlyRepository repository , IFileService file)
        {
            _repository = repository;
            _file = file;
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
    }
}
