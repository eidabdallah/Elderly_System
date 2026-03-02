using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderMealRepository : IElderMealRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderMealRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ElderExistsAsync(int elderlyId)
        {
            return await _context.Set<Elderly>()
                .AsNoTracking()
                .AnyAsync(e => e.Id == elderlyId);
        }

        public async Task<List<ElderMeal>> GetMealsByElderAndDateAsync(int elderlyId, DateTime date)
        {
            var d = date.Date;

            return await _context.Set<ElderMeal>()
                .Where(m => m.ElderlyId == elderlyId && m.Date.Date == d)
                .ToListAsync();
        }

        public async Task AddMealsAsync(List<ElderMeal> meals)
        {
            await _context.Set<ElderMeal>().AddRangeAsync(meals);
        }
        public async Task<List<ElderlyMiniDto>> GetElderliesMiniAsync(int? elderlyId = null)
        {
            var q = _context.Set<Elderly>().AsNoTracking().AsQueryable();

            if (elderlyId.HasValue)
                q = q.Where(e => e.Id == elderlyId.Value);

            return await q
                .OrderBy(e => e.Name)
                .Select(e => new ElderlyMiniDto
                {
                    Id = e.Id,
                    Name = e.Name ?? ""
                })
                .ToListAsync();
        }

        public async Task<List<ElderMealMiniDto>> GetMealsInRangeAsync(DateTime start, DateTime end, int? elderlyId = null)
        {
            var s = start.Date;
            var eExclusive = end.Date.AddDays(1);

            var q = _context.Set<ElderMeal>()
                .AsNoTracking()
                .Where(m => m.Date >= s && m.Date < eExclusive);

            if (elderlyId.HasValue)
                q = q.Where(m => m.ElderlyId == elderlyId.Value);

            return await q.Select(m => new ElderMealMiniDto
            {
                ElderlyId = m.ElderlyId,
                Date = m.Date.Date,
                MealType = m.MealType,
                MealDetails = m.MealDetails
            }).ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
