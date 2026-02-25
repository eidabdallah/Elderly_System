using Elderly_System.DAL.DTO.Response.Doctor;

namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseDiagnosisDto
    {
        public int ReportId { get; set; }
        public string Date { get; set; } = null!;
        public string DiagnosisUrl { get; set; } = null!;
        public string DiagnosisPublicId { get; set; } = null!;
        public DoctorInfoDto Doctor { get; set; } = null!;
    }
}
