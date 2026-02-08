using Elderly_System.DAL.Enums;

namespace ElderlySystem.DAL.Model
{
    public class Employee : ApplicationUser
    {
        public string? JobTitle { get; set; } = null!;
        public DateTime? HireDate { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public string? FieldOfStudy { get; set; }
        public float? YearsOfStudy { get; set; }
        public string? YearOfGraduation { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<WorkExperience>? WorkExperiences { get; set; } = new List<WorkExperience>();
    }
}

