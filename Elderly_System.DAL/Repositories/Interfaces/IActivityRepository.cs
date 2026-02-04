using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task AddActivityAsync(Activity activity);
        Task<Activity?> GetActivityByIdAsync(int id);
        Task<List<Activity>> GetAllActivitiesAsync();
        Task DeleteActivityAsync(Activity activity);
        Task UpdateActivityAsync(Activity activity);
        Task UpdateParticipantAsync(Participant participant);
    }
}
