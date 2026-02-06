using Elderly_System.DAL.DTO.Request.Vistor;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IVistorService
    {
        // sponsor
        Task<ServiceResult> AddVisitorAsync(string sponsorId, AddVisitorRequest request);

        // admin
        Task<ServiceResult> GetPendingRequestsAsync();
        Task<ServiceResult> ReplyAsync(int requestId, AdminVisitorReplyRequest request);
    }
}
