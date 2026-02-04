using EderlySystem.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Employee : ApplicationUser
    {
        [Required(ErrorMessage = "المسمّى الوظيفي مطلوب.")]
        public string JobTitle { get; set; } = null!;
        [Required(ErrorMessage = "تاريخ التعيين مطلوب.")]
        public DateTime HireDate { get; set; }
        [Required(ErrorMessage = "المستوى التعليمي مطلوب.")]
        public EducationLevel EducationLevel { get; set; }
        [StringLength(100, ErrorMessage = "حقل التخصص لا يجب أن يتجاوز 100 حرف.")]
        public string? FieldOfStudy { get; set; }

        [Range(0, 20, ErrorMessage = "سنوات الدراسة يجب أن تكون بين 0 و 20.")]
        public float? YearsOfStudy { get; set; }

        [StringLength(50, ErrorMessage = "الدرجة العلمية لا يجب أن تتجاوز 50 حرف.")]
        public string? AcademicDegree { get; set; }
        public DateTime? YearDfGraduation { get; set; }
        [Required(ErrorMessage = "الحالة الاجتماعية مطلوبة.")]
        public MaritalStatus MaritalStatus { get; set; }
        public DateTime? EndDate { get; set; }
        [MinLength(1, ErrorMessage = "يجب إدخال مهارة واحدة على الأقل.")]
        public ICollection<string> Skills { get; set; } = new List<string>();
        public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
    }
}

