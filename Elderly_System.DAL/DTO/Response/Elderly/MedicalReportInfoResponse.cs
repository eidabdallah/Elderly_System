namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class MedicalReportInfoResponse
    {
        public int ReportId { get; set; }
        public string Date { get; set; }
        public string DiagnosisUrl { get; set; } = null!;
        public DoctorInfoResponse Doctor { get; set; } = null!;
    }
}
