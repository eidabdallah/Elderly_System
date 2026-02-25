using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlyNurseService : IElderlyNurseService
    {
        private readonly IElderlyNurseRepository _repository;
        private readonly IFileService _file;

        public ElderlyNurseService(IElderlyNurseRepository repository , IFileService file)
        {
            _repository = repository;
            _file = file;
        }
        private static bool HasAllowedExt(IFormFile file, params string[] allowed)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowed.Contains(ext);
        }
        public async Task<ServiceResult> GetActiveResidentElderliesAsync()
        {
            var data = await _repository.GetActiveResidentElderliesAsync();
            return ServiceResult.SuccessWithData(data, "تم جلب المسنين بنجاح");
        }
        public async Task<ServiceResult> GetElderlyDetailsAsync(int elderlyId)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            var dto = await _repository.GetElderlyDetailsAsync(elderlyId);

            if (dto == null)
                return ServiceResult.Failure("المسن غير موجود .");

            return ServiceResult.SuccessWithData(dto, "تم جلب تفاصيل المسن بنجاح");
        }
        public async Task<ServiceResult> GetMedicalReportDiagnosisAsync(int reportId)
        {
            if (reportId <= 0) return ServiceResult.Failure("رقم التقرير غير صحيح.");

            var report = await _repository.GetMedicalReportByIdAsync(reportId);
            if (report == null) return ServiceResult.Failure("التقرير غير موجود.");

            var dto = new NurseDiagnosisDto
            {
                ReportId = report.Id,
                Date = report.Date.ToString("yyyy-MM-dd"),
                DiagnosisUrl = report.DiagnosisUrl,
                DiagnosisPublicId = report.DiagnosisPublicId,
                Doctor = new DoctorInfoDto
                {
                    DoctorId = report.Doctor.Id,
                    Name = report.Doctor.Name,
                    WorkPlace = report.Doctor.WorkPlace,
                    Phone = report.Doctor.Phone
                }
            };

            return ServiceResult.SuccessWithData(dto, "تم جلب التشخيص بنجاح");
        }
        public async Task<ServiceResult> UploadComprehensiveExamAsync(int elderlyId, UploadComprehensiveExamRequest request)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            if (request.File is null)
                return ServiceResult.Failure("صورة الفحص الشامل مطلوبة.");

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            if (!HasAllowedExt(request.File, allowed))
                return ServiceResult.Failure("الفحص الشامل يجب أن يكون صورة أو ملف PDF.");

            var elderly = await _repository.GetByIdAsync(elderlyId);
            if (elderly is null)
                return ServiceResult.Failure("المسن غير موجود.");

            var uploaded = await _file.UploadAsync(request.File, "elderly/comprehensive-exam");

            elderly.ComprehensiveExamination = uploaded.Url;

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم رفع الفحص الشامل بنجاح.");
        }
    }
}
