using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlyNurseService : IElderlyNurseService
    {
        private readonly IElderlyNurseRepository _repository;

        public ElderlyNurseService(IElderlyNurseRepository repository)
        {
            _repository = repository;
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
    }
}
