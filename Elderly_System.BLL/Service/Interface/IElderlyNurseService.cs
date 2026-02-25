using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;
using Microsoft.AspNetCore.Http;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlyNurseService
    {
        Task<ServiceResult> GetActiveResidentElderliesAsync();
        Task<ServiceResult> GetElderlyDetailsAsync(int elderlyId);
        Task<ServiceResult> GetMedicalReportDiagnosisAsync(int reportId);
        Task<ServiceResult> UploadComprehensiveExamAsync(int elderlyId, UploadComprehensiveExamRequest request);
        Task<ServiceResult> DeleteComprehensiveExamAsync(int elderlyId);
        Task<ServiceResult> AddDiseasesAsync(int elderlyId, AddDiseasesRequest request);
        Task<ServiceResult> RemoveDiseaseAsync(int elderlyId, RemoveDiseaseRequest request);
        Task<ServiceResult> GetDoctorsAsync();
        Task<ServiceResult> AddMedicalReportAsync(int elderlyId, AddMedicalReportRequest request);
    }
}
