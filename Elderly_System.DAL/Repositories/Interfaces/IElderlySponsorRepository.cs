using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlySponsorRepository
    {
        Task<bool> IsElderlyNationalIdExistsAsync(string nationalId);
        Task AddAsync(Elderly elderly, Doctor doctor, MedicalReport report, ElderlySponsor link);
        Task<int?> GetElderlyIdForSponsorAsync(string sponsorId);
        Task<bool> LinkExistsAsync(int elderlyId, string sponsorId);
        Task AddLinkAsync(int elderlyId, string sponsorId, string kinShip, string degree);
        Task<int?> GetElderlyIdByNationalIdAsync(string nationalId);
        Task<string?> GetSponsorIdByNationalIdAsync(string nationalId);
        Task<bool> IsLinkBetweenAsync(int elderlyId, string sponsorId);
        Task CreateLinkAsync(int elderlyId, string sponsorId, string kinShip, string degree);
    }
}
