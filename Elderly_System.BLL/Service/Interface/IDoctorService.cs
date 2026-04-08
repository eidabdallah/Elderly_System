using Elderly_System.DAL.DTO.Request.Doctor;
using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IDoctorService
    {
        Task<ServiceResult> GetMyElderliesAsync(string doctorId);
        Task<ServiceResult> AddMedicalReportAsync(string doctorId, AddMedicalReportDto dto);
        Task<ServiceResult> GetPendingDoctorRequestsAsync(string doctorId);
        Task<ServiceResult> UpdateDoctorRequestStatusAsync(string doctorId, int requestId, bool isApproved);
        Task<ServiceResult> GetDoctorProfileAsync(string doctorId);
        Task<ServiceResult> UpdateDoctorProfileAsync(string doctorId, UpdateDoctorProfileRequest request);
    }
}
