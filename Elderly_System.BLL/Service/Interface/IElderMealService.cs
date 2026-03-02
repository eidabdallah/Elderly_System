using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderMealService
    {
        Task<ServiceResult> AddDailyMealsWithDetailsAsync(AddDailyMealsRequest request);
        Task<ServiceResult> GetWeeklyMealsAsync(int offset = 0, int? elderlyId = null);

    }
}
