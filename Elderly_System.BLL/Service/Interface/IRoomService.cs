using Elderly_System.DAL.DTO.Request.Room;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IRoomService
    {
        Task<ServiceResult> AddRoomAsync(RoomCreateRequest request);
        Task<ServiceResult> GetRoomByIdAsync(int id);
        Task<ServiceResult> GetAllRoomAsync();
        Task<ServiceResult> DeleteRoomAsync(int id);
        Task<ServiceResult> ToggleRoomStatusAsync(int roomId);
    }
}
