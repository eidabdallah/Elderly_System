using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Nurse
{
    public class CompleteNurseProfileRequest
    {
        [Required(ErrorMessage = "المستوى التعليمي مطلوب")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationLevel EducationLevel { get; set; }

        [Required(ErrorMessage = "الحالة الاجتماعية مطلوبة")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaritalStatus MaritalStatus { get; set; }

        [Required(ErrorMessage = "مجال الدراسة مطلوب")]
        [StringLength(100, ErrorMessage = "مجال الدراسة يجب ألا يزيد عن 100 حرف")]
        public string FieldOfStudy { get; set; } = null!;

        [Range(0, 20, ErrorMessage = "عدد سنوات الدراسة يجب أن يكون بين 0 و 20")]
        public float YearsOfStudy { get; set; }

        [Required(ErrorMessage = "سنة التخرج مطلوبة")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "سنة التخرج يجب أن تكون 4 أرقام مثل 2022")]
        public string YearOfGraduation { get; set; } = null!;
        public List<WorkExperienceRequest>? WorkExperiences { get; set; } = new List<WorkExperienceRequest>();

    }

}
