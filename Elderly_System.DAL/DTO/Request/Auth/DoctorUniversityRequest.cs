using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Auth
{
    public class DoctorUniversityRequest
    {
        [Required(ErrorMessage = "اسم الجامعة مطلوب.")]
        public string UniversityName { get; set; } = null!;

        [Required(ErrorMessage = "الدرجة العلمية مطلوبة.")]
        public DegreeEducation Degree { get; set; }
    }
}
