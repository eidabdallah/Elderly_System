using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Vistor;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;

namespace Elderly_System.BLL.Service.Classes
{
    public class VistorService : IVistorService
    {
        private readonly IVistorRepository _repository;

        public VistorService(IVistorRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> AddVisitorAsync(string sponsorId, AddVisitorRequest request)
        {
            var elderlyId = await _repository.GetElderlyIdForSponsorAsync(sponsorId);
            if (elderlyId is null)
                return ServiceResult.Failure("لا يوجد مسن مرتبط بهذا الكفيل.");

            var today = DateTime.Today;

            if (request.Date.Date < today)
                return ServiceResult.Failure("تاريخ الزيارة لا يمكن أن يكون قبل تاريخ اليوم.");

            if (request.EndTime <= request.StartTime)
                return ServiceResult.Failure("وقت نهاية الزيارة يجب أن يكون بعد وقت البداية.");

            if (request.Date.Date == today)
            {
                var now = DateTime.Now.TimeOfDay;
                if (request.StartTime <= now)
                    return ServiceResult.Failure("وقت بداية الزيارة يجب أن يكون بعد الوقت الحالي.");
            }
            var duration = request.EndTime - request.StartTime;
            if (duration.TotalMinutes < 15)
                return ServiceResult.Failure("مدة الزيارة قصيرة جدًا.");

            if (duration.TotalHours > 4)
                return ServiceResult.Failure("مدة الزيارة طويلة جدًا.");


            var visitor = await _repository.GetVisitorByPhoneAsync(request.Phone);
            if (visitor is null)
            {
                visitor = new Visitor
                {
                    Name = request.Name,
                    Phone = request.Phone
                };
                await _repository.AddVisitorAsync(visitor);
            }

            var exists = await _repository.ExistsElderlyVisitorAsync(elderlyId.Value, visitor.Id, request.Date, request.StartTime, request.EndTime);
            if (exists)
                return ServiceResult.Failure("تم إضافة نفس طلب الزيارة مسبقاً.");

            var link = new ElderlyVisitor
            {
                ElderlyId = elderlyId.Value,
                VisitorId = visitor.Id,
                Date = request.Date.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
            };
            await _repository.AddElderlyVisitorAsync(link);
            return ServiceResult.SuccessMessage("تم إرسال طلب الزيارة بنجاح (بانتظار موافقة الإدارة).");
        }
    }
}
