using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.Model
{
    public class DoctorUniversity
    {
        public int Id { get; set; }
        public string UniversityName { get; set; } = null!;
        public DegreeEducation Degree { get ; set; }
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
