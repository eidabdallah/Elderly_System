using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlyRepository
    {
        // sponsor
        Task<bool> IsElderlyNationalIdExistsAsync(string nationalId);
        Task AddAsync(Elderly elderly, Doctor doctor, MedicalReport report, ElderlySponsor link);
    }
}
