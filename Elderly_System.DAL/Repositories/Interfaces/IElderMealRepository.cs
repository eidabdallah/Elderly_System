using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderMealRepository
    {
        Task<bool> ElderExistsAsync(int elderlyId);

        Task<List<ElderMeal>> GetMealsByElderAndDateAsync(int elderlyId, DateTime date);

        Task AddMealsAsync(List<ElderMeal> meals);
        Task<List<ElderlyMiniDto>> GetElderliesMiniAsync(int? elderlyId = null);

        Task<List<ElderMealMiniDto>> GetMealsInRangeAsync(DateTime start, DateTime end, int? elderlyId = null);

        Task<int> SaveChangesAsync();
    }
}
