using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlyNurseRepository
    {
        Task<List<NurseElderlyListDto>> GetActiveResidentElderliesAsync();
        Task<NurseElderlyDetailsDto?> GetElderlyDetailsAsync(int elderlyId);
        Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId);
        Task<Elderly?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<Doctor?> GetDoctorByIdAsync(int doctorId);
        Task<bool> DoctorPhoneExistsAsync(string phone);
        Task AddMedicalReportAsync(MedicalReport report);
        Task AddDoctorAsync(Doctor doctor);
        Task<List<DoctorInfoDto>> GetDoctorsAsync();
    }
}
