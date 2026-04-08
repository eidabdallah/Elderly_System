using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
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

            var list = await _repository.GetMyElderliesAsync(sponsorId);
            return ServiceResult.SuccessWithData(list, "تم جلب بيانات المسنين والكفلاء بنجاح.");
        }
        public async Task<ServiceResult> GetElderlyDetailsForSponsorAsync(string sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var elderly = await _repository.GetByIdFullDetailsForSponsorAsync(sponsorId);
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
                        Name = latestReport.Doctor.FullName,
                        WorkPlace = latestReport.Doctor.WorkPlaces.OrderByDescending(wp => wp.Id).Select(wp => wp.WorkPlace).FirstOrDefault() ?? "",

                        Phone = latestReport.Doctor.PhoneNumber!
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
                    Name = report.Doctor.FullName,
                    WorkPlace = report.Doctor.WorkPlaces
                    .OrderByDescending(wp => wp.Id)
                    .Select(wp => wp.WorkPlace)
                    .FirstOrDefault() ?? "",
                    Phone = report.Doctor.PhoneNumber!
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
        public async Task<ServiceResult> GetCurrentDoctorAsync(string sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var elderly = await _repository.GetElderlyWithCurrentDoctorBySponsorIdAsync(sponsorId);

            if (elderly == null)
                return ServiceResult.Failure("لا يوجد مسن مرتبط بهذا الكفيل.");

            if (elderly.Doctor == null)
                return ServiceResult.Failure("لا يوجد دكتور حالي مرتبط بهذه المسنة.");

            var dto = new CurrentDoctorResponse
            {
                Name = elderly.Doctor.FullName ?? "",
                Phone = elderly.Doctor.PhoneNumber ?? "",
                WorkPlace = elderly.Doctor.WorkPlaces
                    .OrderByDescending(wp => wp.Id)
                    .Select(wp => wp.WorkPlace)
                    .FirstOrDefault() ?? ""
            };

            return ServiceResult.SuccessWithData(dto, "تم جلب معلومات الدكتور الحالي بنجاح.");
        }
        public async Task<ServiceResult> GetAvailableDoctorsAsync(string sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            var hasElderly = await _repository.GetMyElderliesAsync(sponsorId);
            if (hasElderly == null)
                return ServiceResult.Failure("لا يوجد مسن مرتبط بهذا الكفيل.");

            var doctors = await _repository.GetAvailableDoctorsForSponsorAsync(sponsorId);

            return ServiceResult.SuccessWithData(doctors, "تم جلب أسماء الدكاترة بنجاح.");
        }
        public async Task<ServiceResult> CreateDoctorChangeRequestAsync(string sponsorId, CreateDoctorChangeRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return ServiceResult.Failure("تعذر تحديد الكفيل من التوكن.");

            if (dto == null || string.IsNullOrWhiteSpace(dto.RequestedDoctorId))
                return ServiceResult.Failure("معرّف الدكتور المطلوب غير صحيح.");

            var elderly = await _repository.GetElderlyBySponsorIdAsync(sponsorId);
            if (elderly == null)
                return ServiceResult.Failure("لا يوجد مسن مرتبط بهذا الكفيل.");

            var doctorExists = await _repository.DoctorExistsAsync(dto.RequestedDoctorId);
            if (!doctorExists)
                return ServiceResult.Failure("الدكتور المطلوب غير موجود.");

            if (!string.IsNullOrWhiteSpace(elderly.DoctorId) && elderly.DoctorId == dto.RequestedDoctorId)
                return ServiceResult.Failure("هذا الدكتور هو الدكتور الحالي بالفعل.");

            var hasPendingRequest = await _repository.HasPendingDoctorChangeRequestAsync(elderly.Id);
            if (hasPendingRequest)
                return ServiceResult.Failure("يوجد طلب تغيير دكتور قيد الانتظار لهذه المسنة.");

            var request = new DoctorChangeRequest
            {
                ElderlyId = elderly.Id,
                RequestedDoctorId = dto.RequestedDoctorId,
                RequestStatus = Status.Pending
            };

            await _repository.AddDoctorChangeRequestAsync(request);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessWithData(request.Id, "تم إرسال طلب تغيير الدكتور بنجاح.");
        }

    }
}
