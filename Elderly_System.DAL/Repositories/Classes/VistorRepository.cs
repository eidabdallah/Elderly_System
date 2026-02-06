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
        {
            return await _context.Visitors.FirstOrDefaultAsync(v => v.Phone == phone);
        }

        public async Task AddVisitorAsync(Visitor visitor)
        {
            await _context.Visitors.AddAsync(visitor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsElderlyVisitorAsync(int elderlyId, int visitorId, DateTime date, TimeSpan start, TimeSpan end)
        {
            return await _context.ElderlyVisitors.AnyAsync(ev =>
                ev.ElderlyId == elderlyId &&
                ev.VisitorId == visitorId &&
                ev.Date.Date == date.Date &&
                ev.StartTime == start &&
                ev.EndTime == end
            );
        }

        public async Task AddElderlyVisitorAsync(ElderlyVisitor link)
        {
            await _context.ElderlyVisitors.AddAsync(link);
            await _context.SaveChangesAsync();
        }
    }
}
