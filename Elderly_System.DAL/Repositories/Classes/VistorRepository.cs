using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class VistorRepository : IVistorRepository
    {
        private readonly ApplicationDbContext _context;

        public VistorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int?> GetElderlyIdForSponsorAsync(string sponsorId)
        {
            return await _context.ElderlySponsors
                .Where(x => x.SponsorId == sponsorId)
                .Select(x => (int?)x.ElderlyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Visitor?> GetVisitorByPhoneAsync(string phone)
            => await _context.Visitors.FirstOrDefaultAsync(v => v.Phone == phone);

        public async Task AddVisitorAsync(Visitor visitor)
        {
            await _context.Visitors.AddAsync(visitor);
            await _context.SaveChangesAsync();
        }

        public async Task AddElderlyVisitorAsync(ElderlyVisitor link)
        {
            await _context.ElderlyVisitors.AddAsync(link);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ElderlyVisitor>> GetPendingRequestsAsync()
        {
            return await _context.ElderlyVisitors
                .Include(x => x.Elderly)
                .Include(x => x.Visitor)
                .Where(x => x.Status == Status.Pending)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<ElderlyVisitor?> GetRequestByIdAsync(int requestId)
        {
            return await _context.ElderlyVisitors
                .Include(x => x.Elderly)
                .Include(x => x.Visitor)
                .FirstOrDefaultAsync(x => x.Id == requestId);
        }

        public async Task UpdateAsync(ElderlyVisitor req)
        {
            _context.ElderlyVisitors.Update(req);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetSponsorEmailsByElderlyIdAsync(int elderlyId)
        {
            return await _context.ElderlySponsors
                .Where(es => es.ElderlyId == elderlyId)
                .Select(es => es.Sponsor.Email!)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct()
                .ToListAsync();
        }
    }
}
