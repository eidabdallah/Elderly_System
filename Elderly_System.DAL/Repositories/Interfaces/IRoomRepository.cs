using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task AddRoomAsync(Room room);
        Task<bool> CheckRoomNumberAsync(int RoomNumber);
    }
}
