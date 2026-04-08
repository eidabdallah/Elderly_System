using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<List<DoctorElderlyResponse>> GetMyElderliesAsync(string doctorId);
        Task<bool> DoctorOwnsElderlyAsync(string doctorId, int elderlyId);
        Task AddMedicalReportAsync(MedicalReport report);
        Task<List<DoctorPendingRequestResponse>> GetPendingDoctorRequestsAsync(string doctorId);
        Task<DoctorChangeRequest?> GetDoctorRequestByIdAsync(int requestId, string doctorId);
        Task<Doctor?> GetDoctorWithDetailsByIdAsync(string doctorId);
        Task SaveChangesAsync();
    }
}
