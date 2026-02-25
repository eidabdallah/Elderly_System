using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.MedicalReport;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderlyNurseRepository : IElderlyNurseRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderlyNurseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<NurseElderlyListDto>> GetActiveResidentElderliesAsync()
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Where(e =>
                    e.status == Status.Active &&
                    e.ResidentStays.Any(rs => rs.Status == Status.Active))
                .Select(e => new NurseElderlyListDto
                {
                    ElderlyId = e.Id,
                    Name = e.Name,
                    RoomNumber = e.ResidentStays
                        .Where(rs => rs.Status == Status.Active)
                        .Select(rs => rs.Room.RoomNumber) 
                        .FirstOrDefault()
                })
                .OrderBy(x => x.RoomNumber)
                .ToListAsync();
        }
        public async Task<NurseElderlyDetailsDto?> GetElderlyDetailsAsync(int elderlyId)
        {
            var data = await _context.Elderlies
                .AsNoTracking()
                .Where(e => e.Id == elderlyId)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.NationalId,
                    e.HealthStatus,
                    e.Diseases,
                    e.Age,
                    e.ComprehensiveExamination,

                    RoomNumber = e.ResidentStays
                        .Where(rs => rs.Status == Status.Active)
                        .OrderByDescending(rs => rs.StartDate)
                        .Select(rs => rs.Room.RoomNumber)
                        .FirstOrDefault(),

                    DiagnosisDates = e.MedicalReports
                        .OrderByDescending(mr => mr.Date)
                        .Select(mr => new MedicalReportDateResponse
                        {
                            ReportId = mr.Id,
                            Date = mr.Date.ToString("yyyy-MM-dd")
                        })
                        .ToList(),

                    LatestDiagnosis = e.MedicalReports
                        .OrderByDescending(mr => mr.Date)
                        .Select(mr => new NurseDiagnosisDto
                        {
                            ReportId = mr.Id,
                            Date = mr.Date.ToString("yyyy-MM-dd"),
                            DiagnosisUrl = mr.DiagnosisUrl,
                            DiagnosisPublicId = mr.DiagnosisPublicId,
                            Doctor = new DoctorInfoDto
                            {
                                DoctorId = mr.Doctor.Id,
                                Name = mr.Doctor.Name,
                                WorkPlace = mr.Doctor.WorkPlace,
                                Phone = mr.Doctor.Phone
                            }
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (data == null) return null;

            return new NurseElderlyDetailsDto
            {
                ElderlyId = data.Id,
                Name = data.Name,
                NationalId = data.NationalId,
                RoomNumber = data.RoomNumber,
                Age = data.Age,
                HealthStatus = data.HealthStatus,
                Diseases = data.Diseases?.ToList() ?? new List<string>(),
                ComprehensiveExamination = data.ComprehensiveExamination,

                LatestDiagnosis = data.LatestDiagnosis,
                DiagnosisDates = data.DiagnosisDates
            };
        }
        public async Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId)
        {
            return await _context.MedicalReports
                .AsNoTracking()
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == reportId);
        }
        public async Task<Elderly?> GetByIdAsync(int id)
       => await _context.Elderlies.FirstOrDefaultAsync(e => e.Id == id);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
