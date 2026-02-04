using Elderly_System.DAL.DTO.Request.Activity;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IActivityService
    {
        Task<ServiceResult> CreateActivityAsync(ActivityCreateRequest request, string employeeId);

    }
}
