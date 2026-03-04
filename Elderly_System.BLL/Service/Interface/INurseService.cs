using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface INurseService
    {
        Task<ServiceResult> GetHomeAsync(string nurseId, int graceMinutes = 30, int reminderMinutes = 10, int expiringDays = 3, int activityTake = 20);

    }
}
