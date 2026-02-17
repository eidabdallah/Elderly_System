using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.Enums;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlyAdminService
    {
        Task<ServiceResult> GetElderliesAsync(Status? status);
        Task<ServiceResult> ChangeElderlyStatusAsync(int elderlyId, Status status);
        Task<ServiceResult> GetElderlyDetailsAsync(int elderlyId);
        Task<ServiceResult> AddResidentStayAsync(AddResidentStayRequest req);
        Task<ServiceResult> GetAvailableRoomsAsync();
        Task<ServiceResult> UpdateStayEndDateAsync(int stayId, DateTime? endDate);
        Task<ServiceResult> TransferStayAsync(int stayId, int newRoomId);

    }
}
