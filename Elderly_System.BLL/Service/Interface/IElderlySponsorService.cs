using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IElderlySponsorService
    {
        Task<ServiceResult> GetMyElderliesAsync(string sponsorId);
        Task<ServiceResult> GetElderlyDetailsForSponsorAsync(string sponsorId, int elderlyId);
        Task<ServiceResult> GetMedicalReportDiagnosisAsync(int reportId);
    }
}

