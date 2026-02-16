using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

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

    }
}
