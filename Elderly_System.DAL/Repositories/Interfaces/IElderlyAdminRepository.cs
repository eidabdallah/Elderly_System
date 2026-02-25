using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.DTO.Response.Room;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
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
        Task<List<AvailableRoomResponse>> GetAvailableRoomsAsync();
        Task<ResidentStay?> GetStayByIdAsync(int stayId);
        Task<List<ElderlyResponse>> GetElderliesByStayAsync(StayFilter filter);
        Task<ResidentStay?> GetActiveStayByElderlyIdAsync(int elderlyId);
        Task<List<AvailableRoomResponse>> GetAvailableRoomsExcludingAsync(int excludeRoomId);
        Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId);
    }
}
