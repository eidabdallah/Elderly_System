using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
                    Name = report.Doctor.FullName,
                    WorkPlace = report.Doctor.WorkPlaces.OrderByDescending(wp => wp.Id).Select(wp => wp.WorkPlace).FirstOrDefault() ?? "",
                    Phone = report.Doctor.PhoneNumber!
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
            elderly.ComprehensiveExaminationPublicId = uploaded.PublicId;

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم رفع الفحص الشامل بنجاح.");
        }
        public async Task<ServiceResult> DeleteComprehensiveExamAsync(int elderlyId)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            var elderly = await _repository.GetByIdAsync(elderlyId);
            if (elderly is null)
                return ServiceResult.Failure("المسن غير موجود.");
            if (string.IsNullOrWhiteSpace(elderly.ComprehensiveExamination) &&
                string.IsNullOrWhiteSpace(elderly.ComprehensiveExaminationPublicId))
                return ServiceResult.Failure("لا يوجد فحص شامل لحذفه.");

            if (!string.IsNullOrWhiteSpace(elderly.ComprehensiveExaminationPublicId))
                await _file.DeleteAsync(elderly.ComprehensiveExaminationPublicId);

            elderly.ComprehensiveExamination = null;
            elderly.ComprehensiveExaminationPublicId = null;

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم حذف الفحص الشامل بنجاح.");
        }
        public async Task<ServiceResult> AddDiseasesAsync(int elderlyId, AddDiseasesRequest request)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            if (request?.Diseases == null || request.Diseases.Count == 0)
                return ServiceResult.Failure("يجب إرسال مرض واحد على الأقل.");

            var elderly = await _repository.GetByIdAsync(elderlyId);
            if (elderly is null)
                return ServiceResult.Failure("المسن غير موجود.");

            elderly.Diseases ??= new List<string>();

            var incoming = request.Diseases
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Select(d => d.Trim())
                .Where(d => d.Length > 0)
                .ToList();

            if (incoming.Count == 0)
                return ServiceResult.Failure("قائمة الأمراض غير صالحة.");

            var existing = new HashSet<string>(
                elderly.Diseases.Select(d => d.Trim()),
                StringComparer.OrdinalIgnoreCase
            );

            var added = new List<string>();
            foreach (var d in incoming)
            {
                if (existing.Add(d))
                {
                    elderly.Diseases.Add(d);
                    added.Add(d);
                }
            }

            if (added.Count == 0)
                return ServiceResult.Failure("كل الأمراض المرسلة موجودة مسبقاً.");

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم إضافة الأمراض بنجاح.");
        }
        public async Task<ServiceResult> RemoveDiseaseAsync(int elderlyId, RemoveDiseaseRequest request)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            if (string.IsNullOrWhiteSpace(request.Disease))
                return ServiceResult.Failure("اسم المرض مطلوب.");

            var elderly = await _repository.GetByIdAsync(elderlyId);
            if (elderly is null)
                return ServiceResult.Failure("المسن غير موجود.");

            if (elderly.Diseases is null || elderly.Diseases.Count == 0)
                return ServiceResult.Failure("لا توجد أمراض لحذفها.");

            var disease = request.Disease.Trim();

            var toRemove = elderly.Diseases.FirstOrDefault(d =>
                d.Trim().Equals(disease, StringComparison.OrdinalIgnoreCase));

            if (toRemove is null)
                return ServiceResult.Failure("المرض غير موجود ضمن القائمة.");

            elderly.Diseases.Remove(toRemove);

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم حذف المرض بنجاح.");
        }

    public async Task<ServiceResult> GetDoctorsAsync()
    {
        var docs = await _repository.GetDoctorsAsync();
        return ServiceResult.SuccessWithData(docs, "تم جلب قائمة الأطباء بنجاح");
    }

        public async Task<ServiceResult> AddMedicalReportAsync(int elderlyId, AddMedicalReportRequest request)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            if (request.DiagnosisFile is null)
                return ServiceResult.Failure("ملف التشخيص مطلوب.");

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            if (!HasAllowedExt(request.DiagnosisFile, allowed))
                return ServiceResult.Failure("ملف التشخيص يجب أن يكون صورة أو PDF.");

            if (!DateTime.TryParseExact(
                    request.Date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var reportDate))
            {
                return ServiceResult.Failure("صيغة التاريخ غير صحيحة. استخدم yyyy-MM-dd.");
            }

            var elderly = await _repository.GetByIdAsync(elderlyId);
            if (elderly is null)
                return ServiceResult.Failure("المسن غير موجود.");

            if (string.IsNullOrWhiteSpace(request.DoctorId))
                return ServiceResult.Failure("يرجى اختيار الطبيب من القائمة.");

            var doctor = await _repository.GetDoctorByIdAsync(request.DoctorId);
            if (doctor is null)
                return ServiceResult.Failure("الطبيب المختار غير موجود.");

            var uploaded = await _file.UploadAsync(request.DiagnosisFile, "elderly/diagnosis");

            var report = new MedicalReport
            {
                Date = reportDate,
                ElderlyId = elderlyId,
                DoctorId = doctor.Id,
                DiagnosisUrl = uploaded.Url,
                DiagnosisPublicId = uploaded.PublicId
            };

            await _repository.AddMedicalReportAsync(report);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessWithData(new
            {
                ReportId = report.Id,
                Date = report.Date.ToString("yyyy-MM-dd"),
                DiagnosisUrl = report.DiagnosisUrl,
                DiagnosisPublicId = report.DiagnosisPublicId,
                DoctorId = report.DoctorId
            }, "تم إضافة التشخيص بنجاح.");
        }
    }
}
