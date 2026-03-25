using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Elderly elderly, ElderlySponsor link)
        {
            link.Elderly = elderly;
            await _context.Elderlies.AddAsync(elderly);
            await _context.ElderlySponsors.AddAsync(link);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsElderlyNationalIdExistsAsync(string nationalId)
        {
            return await _context.Elderlies.AnyAsync(e => e.NationalId == nationalId);
        }
        public async Task<Elderly?> GetActiveElderlyByNationalIdAsync(string nationalId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.NationalId == nationalId && e.status == Status.Active);
        }

        public async Task<bool> IsSponsorLinkedToElderlyAsync(int elderlyId, string sponsorId)
        {
            return await _context.ElderlySponsors
                .AnyAsync(x => x.ElderlyId == elderlyId && x.SponsorId == sponsorId);
        }

        public async Task AddElderlySponsorLinkAsync(ElderlySponsor link)
        {
            _context.ElderlySponsors.Add(link);
            await _context.SaveChangesAsync();
        }
    }
}
