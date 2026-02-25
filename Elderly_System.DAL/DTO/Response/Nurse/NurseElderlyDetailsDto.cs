using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.MedicalReport;

namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseElderlyDetailsDto
    {
        public int ElderlyId { get; set; }
        public string Name { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public int RoomNumber { get; set; }
        public int Age { get; set; }
        public string HealthStatus { get; set; } = null!;
        public List<string> Diseases { get; set; } = new();

        public string? ComprehensiveExamination { get; set; }
        public NurseDiagnosisDto? LatestDiagnosis { get; set; }
        public List<MedicalReportDateResponse> DiagnosisDates { get; set; } = new();
    }
}
