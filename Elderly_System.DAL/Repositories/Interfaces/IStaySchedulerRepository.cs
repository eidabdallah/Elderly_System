using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IStaySchedulerRepository
    {
        Task<List<ResidentStay>> GetStaysToAutoFinishAsync(DateTime today);
        Task SaveChangesAsync();


    }
}
