using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.DTO.Response.MedicalReport;
using Elderly_System.DAL.DTO.Response.Room;
using Elderly_System.DAL.DTO.Response.Sponsor;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlySponsorService : IElderlySponsorService
    {
        private readonly IElderlySponsorRepository _repository;

        public ElderlySponsorService(IElderlySponsorRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> GetMyElderliesAsync(string sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var list = await _repository.GetMyElderliesWithAllSponsorsAsync(sponsorId);
            return ServiceResult.SuccessWithData(list, "تم جلب بيانات المسنين والكفلاء بنجاح.");
        }
        public async Task<ServiceResult> GetElderlyDetailsForSponsorAsync(string sponsorId, int elderlyId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var elderly = await _repository.GetByIdFullDetailsForSponsorAsync(elderlyId, sponsorId);
            if (elderly == null)
                return ServiceResult.Failure("لا يوجد صلاحية أو المسن غير موجود.");

            var stay = elderly.ResidentStays?
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault(s => s.Status == Status.Active)
                ?? elderly.ResidentStays?.OrderByDescending(s => s.StartDate).FirstOrDefault();

            var reportsOrdered = elderly.MedicalReports?
                .OrderByDescending(r => r.Date)
                .ToList() ?? new List<MedicalReport>();

            var latestReport = reportsOrdered.FirstOrDefault();

            var response = new ElderlyDetailsForSponsorDto
            {
                ElderlyId = elderly.Id,
                ElderlyName = elderly.Name,
                NationalId = elderly.NationalId,
                Doctrine = elderly.Doctrine,
                MaritalStatus = UserDetailsResponse.ToArabic(elderly.MaritalStatus),
                City = elderly.City,
                Street = elderly.Street,
                HealthStatus = elderly.HealthStatus,
                Diseases = elderly.Diseases?.ToList() ?? new List<string>(),
                BDate = elderly.BDate.ToString("yyyy-MM-dd"),
                Age = elderly.Age,
                ComprehensiveExamination = elderly.ComprehensiveExamination,
                NationalIdImage = elderly.NationalIdImage,
                HealthInsurance = elderly.HealthInsurance,
                ReasonRegister = elderly.ReasonRegister,
                Status = UserResponse.ToArabic(elderly.status),

                CurrentStay = (stay == null || stay.Room == null) ? null : new ResidentStayInfoForSponsorResponse
                {
                    StayId = stay.Id,
                    StartDate = stay.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = stay.EndDate == null ? "مستمر" : stay.EndDate.Value.ToString("yyyy-MM-dd"),
                    Status = UserDetailsResponse.ToArabic(stay.Status),

                    Room = new RoomShortSponsorResponse
                    {
                        RoomNumber = stay.Room.RoomNumber,
                        RoomType = stay.Room.RoomType,
                        Images = stay.Room.RoomImages
                            .OrderBy(x => x.Id)
                            .Select(img => new RoomImageResponse
                            {
                                Url = img.Url,
                            })
                            .ToList()
                    }
                },

                LatestMedicalReport = latestReport == null ? null : new MedicalReportInfoResponse
                {
                    ReportId = latestReport.Id,
                    Date = latestReport.Date.ToString("yyyy-MM-dd"),
                    DiagnosisUrl = latestReport.DiagnosisUrl,
                    Doctor = new DoctorInfoResponse
                    {
                        DoctorId = latestReport.Doctor.Id,
                        Name = latestReport.Doctor.Name,
                        WorkPlace = latestReport.Doctor.WorkPlace,
                        Phone = latestReport.Doctor.Phone
                    }
                },

                MedicalReportDates = reportsOrdered
                    .Select(r => new MedicalReportDateResponse
                    {
                        ReportId = r.Id,
                        Date = r.Date.ToString("yyyy-MM-dd")
                    })
                    .ToList()
            };

            return ServiceResult.SuccessWithData(response, "تم جلب تفاصيل المسن بنجاح");
        }
        public async Task<ServiceResult> GetMedicalReportDiagnosisAsync(int reportId)
        {
            if (reportId <= 0)
                return ServiceResult.Failure("رقم التقرير غير صحيح.");

            var report = await _repository.GetMedicalReportByIdAsync(reportId);
            if (report == null)
                return ServiceResult.Failure("التقرير غير موجود.");

            var dto = new MedicalReportDiagnosisResponse
            {
                ReportId = report.Id,
                Date = report.Date.ToString("yyyy-MM-dd"),
                DiagnosisUrl = report.DiagnosisUrl,
                DiagnosisPublicId = report.DiagnosisPublicId,
                Doctor = new DoctorInfoResponse
                {
                    DoctorId = report.Doctor.Id,
                    Name = report.Doctor.Name,
                    WorkPlace = report.Doctor.WorkPlace,
                    Phone = report.Doctor.Phone
                }
            };

            return ServiceResult.SuccessWithData(dto, "تم جلب التشخيص بنجاح");
        }
        public async Task<ServiceResult> GetMyElderliesMedicinesAsync(string sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var data = await _repository.GetMyElderliesMedicinesAsync(sponsorId);
            return ServiceResult.SuccessWithData(data, "تم جلب أدوية المسنين بنجاح");
        }
        public async Task<ServiceResult> GetMyElderliesTodayChecklistsAsync(string sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var data = await _repository.GetMyElderliesTodayChecklistsAsync(sponsorId);
            return ServiceResult.SuccessWithData(data, "تم جلب قائمة المتابعة لليوم بنجاح");
        }

    }
}
