using Elderly_System.DAL.DTO.Response.MedicalReport;
using Elderly_System.DAL.DTO.Response.Nurse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Response.Doctor
{
    public class DoctorElderlyDetailsDto
    {
        public int ElderlyId { get; set; }
        public string Name { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string City { get; set; } = null!;
        public string BDate { get; set; } = null!;
        public string HealthStatus { get; set; } = null!;
        public List<string> Diseases { get; set; } = new();

        public string? ComprehensiveExamination { get; set; }
        public NurseDiagnosisDto? LatestDiagnosis { get; set; }
        public List<MedicalReportDateResponse> DiagnosisDates { get; set; } = new();
    }
}
