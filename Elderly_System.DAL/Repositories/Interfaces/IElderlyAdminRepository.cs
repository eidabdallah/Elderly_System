using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Enums;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlyAdminRepository
    {
        Task<List<ElderlyWithSponsorsDto>> GetAllWithSponsorsAsync(Status status);
        Task<Elderly?> GetByIdAsync(int id);
        Task<Elderly?> GetByIdWithSponsorsAsync(int elderlyId);
        Task SaveChangesAsync();
    }
}
