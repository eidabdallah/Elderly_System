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
    }
}
