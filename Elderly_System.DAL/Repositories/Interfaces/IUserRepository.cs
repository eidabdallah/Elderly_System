using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetUsersAsync(Status? status = null, string? name = null);
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<ApplicationUser?> GetBaseAsync(string id);
        Task<Employee?> GetEmployeeAsync(string id);
        Task<Nurse?> GetNurseAsync(string id);
        Task<Doctor?> GetDoctorAsync(string userId);
        Task<Sponsor?> GetSponsorWithElderlyAsync(string id);
        Task<Nurse?> GetByIdAsync(string id);
        Task UpdateAsync(Nurse nurse);
        Task<Sponsor?> GetSponsorWithElderlyForUpdateAsync(string id);
        Task<bool> DeleteElderlyAndLinkBySponsorIdAsync(string sponsorId);
        Task<bool> DeleteElderlySponsorLinkBySponsorIdAsync(string sponsorId);
        Task<int> SaveChangesAsync();

    }
}
