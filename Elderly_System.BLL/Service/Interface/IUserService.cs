using Elderly_System.DAL.Enums;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IUserService 
    {
        Task<ServiceResult> GetUsersAsync(Status? status = null, Role? role = null);
    }
}
