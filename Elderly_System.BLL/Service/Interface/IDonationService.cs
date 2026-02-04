using Elderly_System.DAL.DTO.Request.Donation;
using ElderlySystem.BLL.Helpers;
using System.Security.Claims;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IDonationService 
    {
        Task<ServiceResult> CreateDonationAsync(DonationCreateRequest request, string userId);

    }
}
