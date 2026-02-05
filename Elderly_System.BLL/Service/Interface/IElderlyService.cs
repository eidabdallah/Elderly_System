using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlyService
    {
        Task<ServiceResult> AddElderlyWithDoctorAsync(string sponsorId, AddElderlyWithDoctorRequest request);

    }
}
