using Elderly_System.DAL.Enums;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IUserService 
    {
        Task<ServiceResult> GetUsersAsync(Status? status = null, Role? role = null);
        Task<ServiceResult> ChangeStatusAsync(string userId, Status newStatus);
        Task<ServiceResult> ChangeRoleAsync(string userId, Role newRole);
        Task<ServiceResult> GetUserDetailsAsync(string userId);


    }
}
