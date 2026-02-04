using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task AddActivityAsync(Activity activity);
        Task<Activity?> GetActivityByIdAsync(int id);
        Task<List<Activity>> GetAllActivitiesAsync();
    }
}
