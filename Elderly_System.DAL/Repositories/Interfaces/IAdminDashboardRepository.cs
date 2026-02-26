using Elderly_System.DAL.DTO.Response.Statistics;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<int> CountElderliesAsync();
        Task<int> CountUsersInRoleAsync(string roleName);
        Task<int> CountSponsorAsync();
        Task<int> CountDonationsAsync();
        Task<int> CountActivitiesAsync();
        Task<int> CountRoomsAsync();
        Task<List<DonationMonthDto>> GetDonationsOverTimeAsync();
    }

}
