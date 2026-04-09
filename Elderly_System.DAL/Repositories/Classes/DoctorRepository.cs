using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.DTO.Response.MedicalReport;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<DoctorElderlyResponse>> GetMyElderliesAsync(string doctorId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Where(e => e.DoctorId == doctorId)
                .OrderBy(e => e.Name)
                .Select(e => new DoctorElderlyResponse
                {
                    ElderlyId = e.Id,
                    ElderlyName = e.Name,
                })
                .ToListAsync();
        }
        public async Task<bool> DoctorOwnsElderlyAsync(string doctorId, int elderlyId)
        {
            return await _context.Elderlies
                .AnyAsync(e => e.Id == elderlyId && e.DoctorId == doctorId);
        }

        public async Task AddMedicalReportAsync(MedicalReport report)
        {
            await _context.MedicalReports.AddAsync(report);
        }
        public async Task<List<DoctorPendingRequestResponse>> GetPendingDoctorRequestsAsync(string doctorId)
        {
            return await _context.DoctorChangeRequests
                .AsNoTracking()
                .Where(r => r.RequestedDoctorId == doctorId && r.RequestStatus == Status.Pending)
                .Include(r => r.Elderly)
                .OrderByDescending(r => r.Id)
                .Select(r => new DoctorPendingRequestResponse
                {
                    RequestId = r.Id,
                    ElderlyId = r.ElderlyId,
                    ElderlyName = r.Elderly.Name,
                    ComprehensiveExamination = r.Elderly.ComprehensiveExamination,
                    RequestStatus = r.RequestStatus.ToString()
                })
                .ToListAsync();
        }

        public async Task<DoctorChangeRequest?> GetDoctorRequestByIdAsync(int requestId, string doctorId)
        {
            return await _context.DoctorChangeRequests
                .Include(r => r.Elderly)
                .FirstOrDefaultAsync(r =>
                    r.Id == requestId &&
                    r.RequestedDoctorId == doctorId &&
                    r.RequestStatus == Status.Pending);
        }
        public async Task<Doctor?> GetDoctorWithDetailsByIdAsync(string doctorId)
        {
            return await _context.Doctors
                .Include(d => d.Specializations)
                .Include(d => d.Diseases)
                .Include(d => d.WorkPlaces)
                .Include(d => d.PreviousWorkPlaces)
                .Include(d => d.OperationTypes)
                .Include(d => d.MedicalProcedures)
                .Include(d => d.DiagnosticTests)
                .Include(d => d.Universities)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
        }
        public async Task<DoctorElderlyDetailsDto?> GetElderlyDetailsAsync(int elderlyId)
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
                    e.BDate,
                    e.City,
                    e.ComprehensiveExamination,

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
                                Name = mr.Doctor.FullName,
                                WorkPlace = mr.Doctor.WorkPlaces
                                    .OrderByDescending(wp => wp.Id)
                                    .Select(wp => wp.WorkPlace)
                                    .FirstOrDefault() ?? "",
                                Phone = mr.Doctor.PhoneNumber!
                            }
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (data == null) return null;

            return new DoctorElderlyDetailsDto
            {
                ElderlyId = data.Id,
                Name = data.Name,
                NationalId = data.NationalId,
                City = data.City,
                BDate = data.BDate.ToString("yyyy-MM-dd") ?? "",
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
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
