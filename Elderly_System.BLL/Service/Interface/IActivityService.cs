using Elderly_System.DAL.DTO.Request.Activity;
using Elderly_System.DAL.DTO.Response.Activity;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IActivityService
    {
        Task<ServiceResult> CreateActivityAsync(ActivityCreateRequest request, string employeeId);
        Task<List<ActivityResponse>> GetAllActivitiesAsync();
        Task<ActivityResponse?> GetActivityByIdAsync(int id);

    }
}
