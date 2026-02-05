using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;
using Microsoft.AspNetCore.Http;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlySponsorService
    {
        Task<ServiceResult> AddElderlyWithDoctorAsync(string sponsorId, AddElderlyWithDoctorRequest request);
        Task<ServiceResult> RegisterCoSponsorAsync(string currentSponsorId, RegisterCoSponsorRequest request, HttpRequest httpRequest);
    }
}

