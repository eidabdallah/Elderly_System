using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Request.Doctor
{
    public class DoctorUniversityRequest
    {
        public string UniversityName { get; set; } = null!;
        public DegreeEducation Degree { get; set; }
    }
}
