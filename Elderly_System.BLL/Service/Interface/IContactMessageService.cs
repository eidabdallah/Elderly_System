using Elderly_System.DAL.DTO.Request.ContactMessage;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IContactMessageService
    {
        Task<ServiceResult> GetAdminMessagesAsync();
        Task<ServiceResult> ReplyAsync(int id, ReplyContactMessageRequest request);
        Task<ServiceResult> AddAsync(AddContactMessageRequest request);

    }
}
