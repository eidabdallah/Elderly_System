using Elderly_System.DAL.DTO.Request.Vistor;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IVistorService
    {
        Task<ServiceResult> AddVisitorAsync(string sponsorId, AddVisitorRequest request);

    }
}
