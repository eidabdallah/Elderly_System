using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderMealService : IElderMealService
    {
        private readonly IElderMealRepository _repository;
        private readonly ApplicationDbContext _context;

        public ElderMealService(IElderMealRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<ServiceResult> AddDailyMealsWithDetailsAsync(AddDailyMealsRequest request)
        {
            if (request == null)
                return ServiceResult.Failure("البيانات غير صحيحة.");

            if (request.ElderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            var date = request.Date.Date;
            if (date < DateTime.Today)
                return ServiceResult.Failure("أضف الاكل بتاريخ اليوم");

            var elderExists = await _repository.ElderExistsAsync(request.ElderlyId);
            if (!elderExists)
                return ServiceResult.Failure("المسن غير موجود.");

            var desired = new Dictionary<MealType, string>
            {
                { MealType.Breakfast, request.Breakfast.Trim() },
                { MealType.Lunch, request.Lunch.Trim() },
                { MealType.Dinner, request.Dinner.Trim() }
            };

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _repository.GetMealsByElderAndDateAsync(request.ElderlyId, date);
                var existingMap = existing.ToDictionary(x => x.MealType, x => x);

                var toAdd = new List<ElderMeal>();

                foreach (var kv in desired)
                {
                    var type = kv.Key;
                    var details = kv.Value;

                    if (existingMap.TryGetValue(type, out var meal))
                    {
                        meal.MealDetails = details;
                        meal.Date = date;
                    }
                    else
                    {
                        toAdd.Add(new ElderMeal
                        {
                            ElderlyId = request.ElderlyId,
                            Date = date,
                            MealType = type,
                            MealDetails = details
                        });
                    }
                }

                if (toAdd.Any())
                    await _repository.AddMealsAsync(toAdd);

                var saved = await _repository.SaveChangesAsync();
                if (saved <= 0)
                {
                    await tx.RollbackAsync();
                    return ServiceResult.Failure("حدث خطأ أثناء حفظ الوجبات.");
                }

                await tx.CommitAsync();
                return ServiceResult.SuccessMessage("تم حفظ وجبات اليوم (فطور/غداء/عشاء) بنجاح.");
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync();
                return ServiceResult.Failure("تعذر الحفظ بسبب تعارض بالبيانات (قد تكون الوجبات مسجلة مسبقاً).");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return ServiceResult.Failure($"حدث خطأ غير متوقع: {ex.Message}");
            }
        }
        public async Task<ServiceResult> GetWeeklyMealsAsync(int offset = 0, int? elderlyId = null)
        {
            var baseDate = DateTime.Now.Date.AddDays(offset * 7);

            var start = GetSaturdayStart(baseDate);
            var dates = Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToList();
            var startDate = dates.First().Date;
            var endDate = dates.Last().Date;

            var dateKeys = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            var elderlies = await _repository.GetElderliesMiniAsync(elderlyId);

            var meals = await _repository.GetMealsInRangeAsync(startDate, endDate, elderlyId);

            var map = new Dictionary<(int ElderlyId, DateTime Day), DailyMealCellDto>();

            foreach (var m in meals)
            {
                var key = (m.ElderlyId, m.Date.Date);
                if (!map.TryGetValue(key, out var cell))
                {
                    cell = new DailyMealCellDto();
                    map[key] = cell;
                }

                switch (m.MealType)
                {
                    case MealType.Breakfast:
                        cell.Breakfast = string.IsNullOrWhiteSpace(m.MealDetails) ? "-" : m.MealDetails;
                        break;
                    case MealType.Lunch:
                        cell.Lunch = string.IsNullOrWhiteSpace(m.MealDetails) ? "-" : m.MealDetails;
                        break;
                    case MealType.Dinner:
                        cell.Dinner = string.IsNullOrWhiteSpace(m.MealDetails) ? "-" : m.MealDetails;
                        break;
                }
            }

            var rows = new List<WeeklyElderMealRowDto>();

            foreach (var e in elderlies)
            {
                var row = new WeeklyElderMealRowDto
                {
                    ElderlyId = e.Id,
                    ElderlyName = e.Name,
                    Days = new Dictionary<string, DailyMealCellDto>()
                };

                foreach (var d in dates)
                {
                    var k = d.ToString("yyyy-MM-dd");
                    row.Days[k] = map.TryGetValue((e.Id, d.Date), out var cell)
                        ? cell
                        : new DailyMealCellDto();
                }

                rows.Add(row);
            }

            var response = new WeeklyElderMealsResponse
            {
                Dates = dateKeys,
                Rows = rows
            };

            return ServiceResult.SuccessWithData(response, "تم جلب جدول الوجبات الأسبوعي بنجاح");
        }

        private static DateTime GetSaturdayStart(DateTime anyDate)
        {
            var d = anyDate.Date;
            int diff = ((int)d.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return d.AddDays(-diff);
        }
    }
}
