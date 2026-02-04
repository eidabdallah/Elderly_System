using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task AddActivityAsync(Activity activity);
    }
}
