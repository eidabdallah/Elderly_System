using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderlyAdminRepository : IElderlyAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderlyAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ElderlyResponse>> GetAllWithSponsorsAsync(Status status)
        {
            return await _context.Elderlies
                .Where(e => e.status == status)
                .AsNoTracking()
                .Select(e => new ElderlyResponse
                {
                    ElderlyId = e.Id,
                    ElderlyName = e.Name,
                    Status = UserResponse.ToArabic(e.status),
                })
                .ToListAsync();
        }

        public async Task<Elderly?> GetByIdAsync(int id)
        {
            return await _context.Elderlies.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Elderly?> GetByIdWithSponsorsAsync(int elderlyId)
        {
            return await _context.Elderlies
                .Include(e => e.ElderlySponsors)
                    .ThenInclude(es => es.Sponsor)
                .FirstOrDefaultAsync(e => e.Id == elderlyId);
        }
        public async Task<Elderly?> GetByIdFullDetailsAsync(int elderlyId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Include(e => e.ElderlySponsors)
                    .ThenInclude(es => es.Sponsor)
                .Include(e => e.ResidentStays)
                    .ThenInclude(rs => rs.Room)
                .Include(e => e.MedicalReports)
                    .ThenInclude(m => m.Doctor)
                .FirstOrDefaultAsync(e => e.Id == elderlyId);
        }
        public async Task<Room?> GetRoomByIdAsync(int roomId)
    => await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);

        public async Task<bool> HasActiveStayAsync(int elderlyId)
            => await _context.ResidentStays.AnyAsync(s => s.ElderlyId == elderlyId && s.Status == Status.Active);

        public async Task AddResidentStayAsync(ResidentStay stay)
            => await _context.ResidentStays.AddAsync(stay);

    }
}
