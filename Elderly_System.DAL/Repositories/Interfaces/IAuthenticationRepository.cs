using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task AddAsync(Elderly elderly , ElderlySponsor link);
        Task<bool> IsElderlyNationalIdExistsAsync(string nationalId);
        Task<Elderly?> GetActiveElderlyByNationalIdAsync(string nationalId);
        Task<bool> IsSponsorLinkedToElderlyAsync(int elderlyId, string sponsorId);
        Task AddElderlySponsorLinkAsync(ElderlySponsor link);
    }
}
