using Elderly_System.DAL.DTO.Request.Nurse;
using ElderlySystem.BLL.Helpers;
using System.Threading.Tasks;

namespace Elderly_System.BLL.Service.Interface
{
    public interface INurseShiftService
    {
        Task<ServiceResult> GetActiveNursesAsync();
        Task<ServiceResult> AssignDailyShiftsAsync(AssignDailyShiftsRequest request);
        Task<ServiceResult> GetScheduleAsync(string? view = "week", DateTime? date = null, int offset = 0);
        Task<ServiceResult> GetMyWeeklyScheduleAsync(string nurseId, int offset = 0);
    }
}
