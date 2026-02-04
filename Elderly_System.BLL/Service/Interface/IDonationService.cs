using Elderly_System.DAL.DTO.Request.Donation;
using Elderly_System.DAL.DTO.Response.Donation;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IDonationService 
    {
        Task<ServiceResult> CreateDonationAsync(DonationCreateRequest request, string AdminId);
        Task<ServiceResult> DeleteDonationAsync(int donationId);
        Task<ServiceResult> UpdateDonationAsync(int donationId, DonationUpdateRequest request);
        Task<List<DonationResponse>> GetAllDonationsAsync();
        Task<DonationResponse?> GetDonationByIdAsync(int id);

    }
}
