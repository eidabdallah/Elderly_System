using Elderly_System.DAL.DTO.Response.Elderly;

namespace Elderly_System.DAL.DTO.Response.MedicalReport
{
    public class MedicalReportDiagnosisResponse
    {
        public int ReportId { get; set; }
        public string Date { get; set; } = null!;
        public string DiagnosisUrl { get; set; } = null!;
        public string DiagnosisPublicId { get; set; } = null!;

        public DoctorInfoResponse Doctor { get; set; } = null!;
    }
}
