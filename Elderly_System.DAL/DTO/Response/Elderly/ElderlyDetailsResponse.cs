using Elderly_System.DAL.DTO.Response.MedicalReport;

namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class ElderlyDetailsResponse
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string Doctrine { get; set; } = null!;
        public string MaritalStatus { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HealthStatus { get; set; } = null!;
        public List<string> Diseases { get; set; } = new();
        public string BDate { get; set; } = null!;
        public int Age { get; set; }
        public string? ComprehensiveExamination { get; set; }
        public string NationalIdImage { get; set; } = null!;
        public string HealthInsurance { get; set; } = null!;
        public string ReasonRegister { get; set; } = null!;
        public string Status { get; set; } = null!;

        public List<ElderlySponsorInfoResponse> Sponsors { get; set; } = new();
        public ResidentStayInfoResponse? CurrentStay { get; set; }

        public MedicalReportInfoResponse? LatestMedicalReport { get; set; }
        public List<MedicalReportDateResponse> MedicalReportDates { get; set; } = new();
    }
}
