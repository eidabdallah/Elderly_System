using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Vistor;
using Elderly_System.DAL.DTO.Response.Vistor;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.BLL.Service.Classes
{
    public class VistorService : IVistorService
    {
        private readonly IVistorRepository _repository;
        private readonly IEmailSender _emailSender;

        public VistorService(IVistorRepository repository , IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
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
            var link = new ElderlyVisitor
            {
                ElderlyId = elderlyId.Value,
                VisitorId = visitor.Id,
                Date = request.Date.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = Status.Pending
            };

            try
            {
                await _repository.AddElderlyVisitorAsync(link);
            }
            catch (DbUpdateException)
            {
                return ServiceResult.Failure("هذا الطلب موجود مسبقاً لنفس الزائر ونفس الموعد.");
            }

            return ServiceResult.SuccessMessage("تم إرسال طلب الزيارة بنجاح (بانتظار موافقة الإدارة).");
        }
        public async Task<ServiceResult> GetPendingRequestsAsync()
        {
            var pending = await _repository.GetPendingRequestsAsync();

            var dto = pending.Select(x => new PendingVisitorResponse
            {
                RequestId = x.Id,

                ElderlyId = x.ElderlyId,
                ElderlyName = x.Elderly?.Name ?? "",

                VisitorId = x.VisitorId,
                VisitorName = x.Visitor?.Name ?? "",
                VisitorPhone = x.Visitor?.Phone ?? "",

                Date = x.Date.ToString("yyyy-MM-dd"),
                StartTime = x.StartTime.ToString(@"hh\:mm"),
                EndTime = x.EndTime.ToString(@"hh\:mm"),

            }).ToList();

            return ServiceResult.SuccessWithData(dto, "تم جلب طلبات الزيارة المعلقة بنجاح.");
        }
        public async Task<ServiceResult> ReplyAsync(int requestId, AdminVisitorReplyRequest request)
        {
            if (request.Status != Status.Active && request.Status != Status.InActive)
                return ServiceResult.Failure("الحالة المسموحة فقط: Active أو InActive.");

            var req = await _repository.GetRequestByIdAsync(requestId);
            if (req is null)
                return ServiceResult.Failure("طلب الزيارة غير موجود.");

            if (req.Status != Status.Pending)
                return ServiceResult.Failure("تم الرد على هذا الطلب مسبقاً.");

            req.Status = request.Status;
            await _repository.UpdateAsync(req);

            var emails = await _repository.GetSponsorEmailsByElderlyIdAsync(req.ElderlyId);

            if (emails.Count > 0)
            {
                var subject = "الرد على طلب الزيارة";
                var statusText = req.Status == Status.Active ? "تمت الموافقة على طلب الزيارة" : "تم رفض طلب الزيارة";

                var body =
                    $"<h3>{statusText}</h3>" +
                    $"<p><b>المسن:</b> {req.Elderly?.Name}</p>" +
                    $"<p><b>الزائر:</b> {req.Visitor?.Name} - {req.Visitor?.Phone}</p>" +
                    $"<p><b>التاريخ:</b> {req.Date:yyyy-MM-dd}</p>" +
                    $"<p><b>الوقت:</b> {req.StartTime:hh\\:mm} - {req.EndTime:hh\\:mm}</p>" +
                    (string.IsNullOrWhiteSpace(request.Message) ? "" : $"<p><b>رسالة الإدارة:</b> {request.Message}</p>");

                foreach (var email in emails)
                    await _emailSender.SendEmailAsync(email, subject, body);
            }

            return ServiceResult.SuccessMessage("تم الرد على الطلب وتحديث الحالة وإرسال الإيميل.");
        }
    }
}
