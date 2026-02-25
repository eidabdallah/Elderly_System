using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlyNurseService
    {
        Task<ServiceResult> GetActiveResidentElderliesAsync();
    }
}
