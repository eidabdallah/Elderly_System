using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task AddRoomAsync(Room room);
        Task<bool> CheckRoomNumberAsync(int RoomNumber);
        Task<Room?> GetRoomByIdWithImagesAsync(int id);
        Task<List<Room>> GetAllRoomAsync();
        Task<bool> DeleteRoomAsync(Room room);
        Task<Room?> GetRoomByIdAsync(int id);
        Task UpdateRoomAsync(Room room);
    }
}
