using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Enums;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlyAdminRepository
    {
        Task<List<ElderlyResponse>> GetAllWithSponsorsAsync(Status status);
        Task<Elderly?> GetByIdAsync(int id);
        Task<Elderly?> GetByIdWithSponsorsAsync(int elderlyId);
        Task SaveChangesAsync();
        Task<Elderly?> GetByIdFullDetailsAsync(int elderlyId);
        Task<Room?> GetRoomByIdAsync(int roomId);
        Task<bool> HasActiveStayAsync(int elderlyId);

        Task AddResidentStayAsync(ResidentStay stay);

    }
}
