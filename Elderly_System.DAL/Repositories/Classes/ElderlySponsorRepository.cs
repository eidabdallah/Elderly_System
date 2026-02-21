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
        public async Task<bool> IsElderlyNationalIdExistsAsync(string nationalId)
        {
            return await _context.Elderlies.AnyAsync(e => e.NationalId == nationalId);
        }

        public async Task AddAsync(Elderly elderly, Doctor doctor, MedicalReport report, ElderlySponsor link)
        {
            report.Elderly = elderly;
            report.Doctor = doctor;
            link.Elderly = elderly;
            await _context.Elderlies.AddAsync(elderly);
            await _context.Doctors.AddAsync(doctor);
            await _context.MedicalReports.AddAsync(report);
            await _context.ElderlySponsors.AddAsync(link);
            await _context.SaveChangesAsync();
        }
        public async Task<int?> GetElderlyIdForSponsorAsync(string sponsorId)
        {
            return await _context.ElderlySponsors
                .Where(x => x.SponsorId == sponsorId)
                .Select(x => (int?)x.ElderlyId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> LinkExistsAsync(int elderlyId, string sponsorId)
        {
            return await _context.ElderlySponsors
                .AnyAsync(x => x.ElderlyId == elderlyId && x.SponsorId == sponsorId);
        }

        public async Task AddLinkAsync(int elderlyId, string sponsorId, string kinShip, string degree)
        {
            var link = new ElderlySponsor
            {
                ElderlyId = elderlyId,
                SponsorId = sponsorId,
                KinShip = kinShip,
                Degree = degree
            };

            await _context.ElderlySponsors.AddAsync(link);
            await _context.SaveChangesAsync();
        }
        public async Task<int?> GetElderlyIdByNationalIdAsync(string nationalId)
        {
            return await _context.Elderlies
                .Where(e => e.NationalId == nationalId)
                .Select(e => (int?)e.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetSponsorIdByNationalIdAsync(string nationalId)
        {
            return await _context.Users
                .Where(u => u.NationalId == nationalId)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsLinkBetweenAsync(int elderlyId, string sponsorId)
        {
            return await _context.ElderlySponsors
                .AnyAsync(x => x.ElderlyId == elderlyId && x.SponsorId == sponsorId);
        }
        public async Task CreateLinkAsync(int elderlyId, string sponsorId, string kinShip, string degree)
        {
            _context.ElderlySponsors.Add(new ElderlySponsor
            {
                ElderlyId = elderlyId,
                SponsorId = sponsorId,
                KinShip = kinShip,
                Degree = degree
            });

            await _context.SaveChangesAsync();
        }
    }
}
