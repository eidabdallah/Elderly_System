namespace Elderly_System.DAL.DTO.Request.Doctor
{
    public class UpdateDoctorProfileRequest
    {
        public string? MedicalRank { get; set; }
        public string? YearsOfExperience { get; set; }
        public int? NumberOfOperations { get; set; }

        public List<string>? Specializations { get; set; }
        public List<string>? Diseases { get; set; }
        public List<string>? WorkPlaces { get; set; }
        public List<string>? OperationTypes { get; set; }
        public List<string>? MedicalProcedures { get; set; }
        public List<string>? DiagnosticTests { get; set; }
    }
}
