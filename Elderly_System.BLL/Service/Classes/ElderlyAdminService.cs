using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlyAdminService : IElderlyAdminService
    {
        private readonly IElderlyAdminRepository _repository;

        public ElderlyAdminService(IElderlyAdminRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> GetElderliesAsync(Status? newStatus)
        {
            if (newStatus != null && (newStatus != Status.Pending && newStatus != Status.Active && newStatus != Status.InActive))
                return ServiceResult.Failure("الحالة المسموحة فقط: انتظار القبول / نشط / غير نشط.");
            var finalStatus = newStatus ?? Status.Active;
            var elderlies = await _repository.GetAllWithSponsorsAsync(finalStatus);
            return ServiceResult.SuccessWithData(elderlies , "تم جلب قائمة المسنين بنجاح");
        }

        public async Task<ServiceResult> ChangeElderlyStatusAsync(int elderlyId, Status status)
        {
            if (status != Status.Active && status != Status.InActive)
                return ServiceResult.Failure("الحالة المسموحة فقط: نشط أو غير نشط");

            var elderly = await _repository.GetByIdWithSponsorsAsync(elderlyId);
            if (elderly == null)
                return ServiceResult.Failure("المسن غير موجود");

            elderly.status = status;

            if (elderly.ElderlySponsors != null && elderly.ElderlySponsors.Any())
            {
                foreach (var link in elderly.ElderlySponsors)
                {
                    if (link.Sponsor != null)
                    {
                        link.Sponsor.Status = status; 
                    }
                }
            }
            await _repository.SaveChangesAsync();
            return ServiceResult.SuccessMessage("تم تغيير حالة المسن  بنجاح");
        }
        public async Task<ServiceResult> GetElderlyDetailsAsync(int elderlyId)
        {
            var elderly = await _repository.GetByIdFullDetailsAsync(elderlyId);
            if (elderly == null)
                return ServiceResult.Failure("المسن غير موجود");

            var stay = elderly.ResidentStays?
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault(s => s.Status == Status.Active)
                ?? elderly.ResidentStays?.OrderByDescending(s => s.StartDate).FirstOrDefault();

            var response = new ElderlyDetailsResponse
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

                Sponsors = elderly.ElderlySponsors?
                    .Select(es => new ElderlySponsorInfoResponse
                    {
                        SponsorName = es.Sponsor != null ? (es.Sponsor.FullName ?? "") : "",
                        KinShip = es.KinShip,
                        Degree = es.Degree
                    })
                    .ToList() ?? new List<ElderlySponsorInfoResponse>(),

                CurrentStay = (stay == null || stay.Room == null) ? null : new ResidentStayInfoResponse
                {
                    StayId = stay.Id,
                    StartDate = stay.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = stay.EndDate == null
                    ? "مستمر"
                    : stay.EndDate.Value.ToString("yyyy-MM-dd"),
                    Status = UserDetailsResponse.ToArabic(stay.Status),

                    Room = new RoomShortResponse
                    {
                        RoomNumber = stay.Room.RoomNumber,
                        RoomType = stay.Room.RoomType,
                    }

                },

                MedicalReports = elderly.MedicalReports?
                    .OrderByDescending(r => r.Date)
                    .Select(r => new MedicalReportInfoResponse
                    {
                        ReportId = r.Id,
                        Date = r.Date.ToString("yyyy-MM-dd"),
                        DiagnosisUrl = r.DiagnosisUrl,
                        Doctor = new DoctorInfoResponse
                        {
                            DoctorId = r.Doctor.Id,
                            Name = r.Doctor.Name,
                            WorkPlace = r.Doctor.WorkPlace,
                            Phone = r.Doctor.Phone
                        }
                    })
                    .ToList() ?? new List<MedicalReportInfoResponse>()
            };

            return ServiceResult.SuccessWithData(response, "تم جلب تفاصيل المسن بنجاح");
        }
        public async Task<ServiceResult> AddResidentStayAsync(AddResidentStayRequest req)
        {
            var elderly = await _repository.GetByIdAsync(req.ElderlyId);
            if (elderly == null)
                return ServiceResult.Failure("المسن غير موجود");

            var room = await _repository.GetRoomByIdAsync(req.RoomId);
            if (room == null)
                return ServiceResult.Failure("الغرفة غير موجودة");

            if (room.Status != Status.Active)
                return ServiceResult.Failure("الغرفة غير متاحة للحجز حاليا");

            if (room.CurrentCapacity >= room.Capacity)
                return ServiceResult.Failure("لا توجد سعة متاحة في هذه الغرفة");

            var hasActive = await _repository.HasActiveStayAsync(req.ElderlyId);
            if (hasActive)
                return ServiceResult.Failure("المسن لديه حجز/إقامة نشطة مسبقا");

            var start =req.StartDate.Date;
            DateTime? end = req.EndDate?.Date;

            if (end != null && end.Value.Date < start)
                return ServiceResult.Failure("تاريخ النهاية لا يمكن أن يكون قبل تاريخ البداية");

            var stay = new ResidentStay
            {
                ElderlyId = req.ElderlyId,
                RoomId = req.RoomId,
                StartDate = start,
                EndDate = end,
                Status = Status.Active
            };

            room.CurrentCapacity += 1;
            if(room.CurrentCapacity == room.Capacity)
                room.Status = Status.Full;

            
            await _repository.AddResidentStayAsync(stay);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم إضافة الحجز للمسن بنجاح");
        }
        public async Task<ServiceResult> GetAvailableRoomsAsync()
        {
            var rooms = await _repository.GetAvailableRoomsAsync();
            return ServiceResult.SuccessWithData(rooms, "تم جلب الغرف المتاحة بنجاح");
        }

        public async Task<ServiceResult> UpdateStayEndDateAsync(int stayId, DateTime? endDate)
        {
            var stay = await _repository.GetStayByIdAsync(stayId);
            if (stay == null)
                return ServiceResult.Failure("الإقامة غير موجودة");

            if (endDate != null && endDate.Value.Date < stay.StartDate.Date)
                return ServiceResult.Failure("تاريخ النهاية لا يمكن أن يكون قبل تاريخ البداية");

            var room = stay.Room;
            if (room == null)
                return ServiceResult.Failure("الغرفة غير موجودة");

            var today = DateTime.Today;

            bool wasCounted = (stay.EndDate == null || stay.EndDate.Value.Date >= today);
            bool willBeCounted = (endDate == null || endDate.Value.Date >= today);

            if (wasCounted && !willBeCounted)
            {
                room.CurrentCapacity = Math.Max(0, room.CurrentCapacity - 1);
                room.Status = (room.CurrentCapacity >= room.Capacity) ? Status.Full : Status.Active;
                stay.Status = Status.Finish;
            }
            else if (!wasCounted && willBeCounted)
            {
                if (room.Status != Status.Active && room.Status != Status.Full)
                    return ServiceResult.Failure("الغرفة غير متاحة للحجز حاليا");

                if (room.CurrentCapacity >= room.Capacity)
                    return ServiceResult.Failure("لا يمكنك الحجز بهذه الغرفة لأنها ممتلئة");

                room.CurrentCapacity += 1;
                room.Status = (room.CurrentCapacity >= room.Capacity) ? Status.Full : Status.Active;
                stay.Status = Status.Active;
            }
            else
            {
                if (willBeCounted)
                    stay.Status = Status.Active;

                if (!willBeCounted)
                    stay.Status = Status.Finish;
            }

            stay.EndDate = endDate?.Date;

            await _repository.SaveChangesAsync();
            return ServiceResult.SuccessMessage("تم تعديل تاريخ النهاية بنجاح");
        }
        public async Task<ServiceResult> TransferStayAsync(int stayId, int newRoomId)
        {
            var stay = await _repository.GetStayByIdAsync(stayId);
            if (stay == null) return ServiceResult.Failure("الإقامة غير موجودة");

            var oldRoom = stay.Room;
            if (oldRoom == null) return ServiceResult.Failure("الغرفة الحالية غير موجودة");

            if (stay.RoomId == newRoomId)
                return ServiceResult.Failure("المسن موجود في نفس الغرفة بالفعل");

            var newRoom = await _repository.GetRoomByIdAsync(newRoomId);
            if (newRoom == null) return ServiceResult.Failure("الغرفة الجديدة غير موجودة");

            if (newRoom.Status != Status.Active && newRoom.Status != Status.Full)
                return ServiceResult.Failure("الغرفة غير متاحة للحجز حاليا");

            if (newRoom.CurrentCapacity >= newRoom.Capacity)
                return ServiceResult.Failure("لا يمكنك الحجز بهذه الغرفة لأنها ممتلئة");

          
            if (stay.Status == Status.Active)
            {
                oldRoom.CurrentCapacity = Math.Max(0, oldRoom.CurrentCapacity - 1);
                oldRoom.Status = (oldRoom.CurrentCapacity >= oldRoom.Capacity) ? Status.Full : Status.Active;
            }

            newRoom.CurrentCapacity += 1;
            newRoom.Status = (newRoom.CurrentCapacity >= newRoom.Capacity) ? Status.Full : Status.Active;

            stay.RoomId = newRoomId;

            await _repository.SaveChangesAsync();
            return ServiceResult.SuccessMessage("تم نقل الغرفة بنجاح");
        }



    }
}
