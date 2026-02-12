using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApplicationUser>> GetUsersAsync(Status? status = null)
        {
            var q = _context.Users.AsNoTracking().AsQueryable();

            if (status is null)
                q = q.Where(u => u.Status == Status.Active);
            else
                q = q.Where(u => u.Status == status.Value);

            return await q.OrderByDescending(u => u.CreatedAt).ToListAsync();
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<ApplicationUser?> GetBaseAsync(string id)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Employee?> GetEmployeeAsync(string id)
        {
            return await _context.Users
                .AsNoTracking()
                .OfType<Employee>()
                .Include(e => e.WorkExperiences)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Nurse?> GetNurseAsync(string id)
        {
            return await _context.Users
                .AsNoTracking()
                .OfType<Nurse>()
                .Include(n => n.WorkExperiences)
                .FirstOrDefaultAsync(n => n.Id == id);
        }


        public async Task<Sponsor?> GetSponsorWithElderlyAsync(string id)
        {
            return await _context.Users.AsNoTracking()
                .OfType<Sponsor>()
                .Include(s => s.ElderlySponsors)
                    .ThenInclude(es => es.Elderly)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Nurse?> GetByIdAsync(string id)
        {
            return await _context.Users
             .OfType<Nurse>()
             .Include(n => n.WorkExperiences)
             .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task UpdateAsync(Nurse nurse)
        {
            _context.Users.Update(nurse);
            await _context.SaveChangesAsync();
        }
    }
}
