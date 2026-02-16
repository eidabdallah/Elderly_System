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
        public async Task<List<ElderlyWithSponsorsDto>> GetAllWithSponsorsAsync(Status status)
        {
            return await _context.Elderlies
                .Where(e => e.status == status)
                .AsNoTracking()
                .Select(e => new ElderlyWithSponsorsDto
                {
                    ElderlyId = e.Id,
                    ElderlyName = e.Name,
                    ReasonRegister = e.ReasonRegister,
                    Status = UserResponse.ToArabic(e.status),
                    Sponsors = e.ElderlySponsors
                        .Select(es => new ElderlySponsorBriefDTO
                        {
                            SponsorName = es.Sponsor.FullName, 
                            KinShip = es.KinShip             
                        })
                        .ToList()
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

    }
}
