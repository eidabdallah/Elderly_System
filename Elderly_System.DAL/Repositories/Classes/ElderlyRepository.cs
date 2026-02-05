using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderlyRepository : IElderlyRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderlyRepository(ApplicationDbContext context)
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
    }
}
