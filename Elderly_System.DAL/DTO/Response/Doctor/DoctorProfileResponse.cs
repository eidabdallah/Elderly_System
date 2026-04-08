namespace Elderly_System.DAL.DTO.Response.Doctor
{
    public class DoctorProfileResponse
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string City { get; set; } = "";
        public string NationalId { get; set; } = "";
        public string Gender { get; set; } = "";
        public string MedicalRank { get; set; } = null!;
        public string YearsOfExperience { get; set; } = null!;
        public int NumberOfOperations { get; set; }
        public DateTime BDate { get; set; }

        public List<string> Specializations { get; set; } = new();
        public List<string> Diseases { get; set; } = new();
        public List<string> WorkPlaces { get; set; } = new();
        public List<string> PreviousWorkPlaces { get; set; } = new();
        public List<string> OperationTypes { get; set; } = new();
        public List<string> MedicalProcedures { get; set; } = new();
        public List<string> DiagnosticTests { get; set; } = new();
        public List<DoctorUniversityResponse> Universities { get; set; } = new();

        public int ElderliesCount { get; set; }
        public int MedicalReportsCount { get; set; }
    }
}
