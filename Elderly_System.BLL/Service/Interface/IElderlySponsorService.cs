using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlySponsorService
    {
        Task<ServiceResult> AddElderlyWithDoctorAsync(string sponsorId, AddElderlyWithDoctorRequest request);
        Task<ServiceResult> VerifyLinkAsync(VerifyElderlySponsorLinkRequest req);
    }
}

