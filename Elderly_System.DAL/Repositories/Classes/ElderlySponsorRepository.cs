using Elderly_System.DAL.DTO.Response.Sponsor;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderlySponsorRepository : IElderlySponsorRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderlySponsorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<SponsorElderlyBriefDto>> GetMyElderliesWithAllSponsorsAsync(string sponsorId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Where(e => e.ElderlySponsors.Any(es => es.SponsorId == sponsorId))
                .Include(e => e.ElderlySponsors)
                    .ThenInclude(es => es.Sponsor)
                .Select(e => new SponsorElderlyBriefDto
                {
                    ElderlyId = e.Id,
                    ElderlyName = e.Name,
                    Sponsors = e.ElderlySponsors
                        .Select(es => new SponsorRelationDto
                        {
                            SponsorId = es.SponsorId,
                            SponsorName = es.Sponsor.FullName ?? "",
                            status = es.Sponsor.Status == Enums.Status.Active ? "نشط" : "لم يتم قبوله بعد",
                            KinShip = es.KinShip,
                            Degree = es.Degree
                        })
                        .OrderBy(x => x.SponsorName)
                        .ToList()
                })
                .OrderBy(x => x.ElderlyName)
                .ToListAsync();
        }
        public async Task<Elderly?> GetByIdFullDetailsForSponsorAsync(int elderlyId, string sponsorId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.Id == elderlyId &&
                            e.ElderlySponsors.Any(es => es.SponsorId == sponsorId))
                .Include(e => e.ResidentStays).ThenInclude(s => s.Room).ThenInclude(r => r.RoomImages)
                .Include(e => e.MedicalReports).ThenInclude(r => r.Doctor)
                .FirstOrDefaultAsync();
        }
        public async Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId)
        {
            return await _context.MedicalReports
                .AsNoTracking()
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == reportId);
        }

    }
}
