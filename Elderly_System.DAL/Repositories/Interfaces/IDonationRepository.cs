using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IDonationRepository
    {
        Task AddDonationAsync(Donation donation);

    }
}
