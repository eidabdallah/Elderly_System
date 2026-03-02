using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.ContactMessage;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Elderly_System.BLL.Service.Classes
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _repository;
        private readonly IEmailSender _emailSender;

        public ContactMessageService(IContactMessageRepository repository , IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }
        public async Task<ServiceResult> GetAdminMessagesAsync()
        {
            var data = await _repository.GetAdminMessagesAsync();
            return ServiceResult.SuccessWithData(data, "تم جلب الرسائل بنجاح.");
        }
        public async Task<ServiceResult> ReplyAsync(int id, ReplyContactMessageRequest request)
        {
            if (id <= 0) return ServiceResult.Failure("رقم الرسالة غير صحيح.");
            if (string.IsNullOrWhiteSpace(request.Reply))
                return ServiceResult.Failure("نص الرد فارغ.");

            var msg = await _repository.GetByIdAsync(id);
            if (msg == null) return ServiceResult.Failure("الرسالة غير موجودة.");

            var subject = $"{msg.Subject}";
            var body = $@"
            <!doctype html>
            <html lang='ar' dir='rtl'>
            <head>
              <meta charset='utf-8'>
              <meta name='viewport' content='width=device-width, initial-scale=1'>
            </head>

            <body style='margin:0;padding:0;background:#f3f4f6;direction:rtl;'>
              <!-- Preheader (hidden) -->
              <div style='display:none;max-height:0;overflow:hidden;opacity:0;color:transparent;'>
                تم إرسال رد الإدارة على رسالتك.
              </div>

              <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background:#f3f4f6;padding:24px 12px;'>
                <tr>
                  <td align='center'>

                    <table role='presentation' width='600' cellpadding='0' cellspacing='0'
                           style='width:600px;max-width:600px;background:#ffffff;border:1px solid #e5e7eb;border-radius:14px;overflow:hidden;
                                  font-family:Tahoma,Arial,sans-serif;color:#111827;'>

                      <!-- Header -->
                      <tr>
                        <td style='background:#0ea5e9;padding:18px 22px;text-align:right;'>
                          <div style='font-size:16px;font-weight:700;color:#ffffff;'>مركز الدعم</div>
                          <div style='font-size:13px;color:rgba(255,255,255,.92);margin-top:4px;'>تمت معالجة رسالتك</div>
                        </td>
                      </tr>

                      <!-- Body -->
                      <tr>
                        <td style='padding:22px;text-align:right;'>

                          <p style='margin:0 0 10px 0;font-size:14px;line-height:1.9;'>
                            مرحبًا <strong style='font-weight:700;'>{msg.FullName}</strong>،
                          </p>

                          <p style='margin:0 0 16px 0;font-size:14px;line-height:1.9;'>
                            شكرًا لتواصلك معنا. فيما يلي رد الإدارة على رسالتك:
                          </p>

                          <!-- Reply card -->
                          <table role='presentation' width='100%' cellpadding='0' cellspacing='0'
                                 style='border:1px solid #e5e7eb;border-radius:12px;background:#f8fafc;'>
                            <tr>
                              <td style='padding:14px 16px;'>
                                <div style='font-size:14px;font-weight:700;margin:0 0 10px 0;color:#0f172a;'>
                                  رد الإدارة
                                </div>
                                <div style='font-size:14px;line-height:1.9;color:#111827;white-space:pre-wrap;'>
                                  {request.Reply}
                                </div>
                              </td>
                            </tr>
                          </table>

                          <div style='height:16px;line-height:16px;'>&nbsp;</div>

                          <!-- Your message card -->
                          <table role='presentation' width='100%' cellpadding='0' cellspacing='0'
                                 style='border:1px solid #e5e7eb;border-radius:12px;background:#ffffff;'>
                            <tr>
                              <td style='padding:14px 16px;'>
                                <div style='font-size:14px;font-weight:700;margin:0 0 10px 0;color:#0f172a;'>
                                  رسالتك
                                </div>

                                <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border-collapse:collapse;'>
                                  <tr>
                                    <td style='padding:6px 0;font-size:13px;color:#6b7280;width:120px;white-space:nowrap;'>الموضوع:</td>
                                    <td style='padding:6px 0;font-size:13px;color:#111827;font-weight:700;'>{msg.Subject}</td>
                                  </tr>
                                  <tr>
                                    <td style='padding:6px 0;font-size:13px;color:#6b7280;width:120px;white-space:nowrap;'>تاريخ الرسالة:</td>
                                    <td style='padding:6px 0;font-size:13px;color:#111827;font-weight:700;'>{msg.CreatedAt:yyyy-MM-dd}</td>
                                  </tr>
                                </table>

                                <div style='margin-top:12px;padding-top:12px;border-top:1px dashed #e5e7eb;
                                            font-size:14px;line-height:1.9;color:#111827;white-space:pre-wrap;'>
                                  {msg.Message}
                                </div>
                              </td>
                            </tr>
                          </table>

                          <p style='margin:18px 0 0 0;font-size:14px;line-height:1.9;'>
                            مع التحية،<br/>
                            <strong>فريق الدعم</strong>
                          </p>

                          <p style='margin:14px 0 0 0;font-size:12px;line-height:1.8;color:#6b7280;'>
                            هذه رسالة تلقائية، يرجى عدم الرد عليها مباشرة.
                          </p>

                        </td>
                      </tr>

                      <!-- Footer -->
                      <tr>
                        <td style='background:#f9fafb;padding:14px 22px;border-top:1px solid #e5e7eb;text-align:center;'>
                          <div style='font-size:12px;line-height:1.7;color:#6b7280;'>
                            © فريق الدعم
                          </div>
                        </td>
                      </tr>

                    </table>

                  </td>
                </tr>
              </table>
            </body>
            </html>";

            await _emailSender.SendEmailAsync(msg.Email, subject, body);

            msg.AdminReply = request.Reply.Trim();
            msg.RepliedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            msg.Status = DAL.Enums.Status.Finish;

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم إرسال الرد عبر البريد وحفظه بنجاح.");
        }
        public async Task<ServiceResult> AddAsync(AddContactMessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length < 2)
                return ServiceResult.Failure("الاسم غير صحيح.");

            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
                return ServiceResult.Failure("البريد الإلكتروني غير صحيح.");

            if (string.IsNullOrWhiteSpace(request.Subject))
                request.Subject = "Contact Message";

            if (string.IsNullOrWhiteSpace(request.Message) || request.Message.Length < 5)
                return ServiceResult.Failure("نص الرسالة قصير جداً.");

            var entity = new ContactMessage
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Subject = request.Subject.Trim(),
                Message = request.Message.Trim(),
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم إرسال رسالتك بنجاح.");
        }
    }
}

