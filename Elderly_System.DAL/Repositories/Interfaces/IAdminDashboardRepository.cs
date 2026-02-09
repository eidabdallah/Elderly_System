namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<int> CountElderliesAsync();
        Task<int> CountUsersInRoleAsync(string roleName);
        Task<int> CountDonationsToDateAsync(DateTime today);
        Task<int> CountEventsToDateAsync(DateTime today);
    }

}
