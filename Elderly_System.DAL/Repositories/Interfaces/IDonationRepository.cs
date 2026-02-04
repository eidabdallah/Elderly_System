using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IDonationRepository
    {
        Task AddDonationAsync(Donation donation);
        Task<Donation?> GetDonationByIdAsync(int id);
        Task DeleteDonationAsync(Donation donation);
    }
}
