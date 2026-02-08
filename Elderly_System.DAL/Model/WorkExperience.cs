using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class WorkExperience
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم جهة العمل مطلوب.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "اسم جهة العمل يجب أن يكون بين 2 و 150 حرف.")]
        public string WorkName { get; set; } = null!;

        [StringLength(150, ErrorMessage = "موقع العمل يجب ألا يتجاوز 150 حرف.")]
        public string WorkLocation { get; set; } = null!;

        [Required(ErrorMessage = "المسمّى الوظيفي مطلوب.")]
        [StringLength(100, ErrorMessage = "المسمّى الوظيفي يجب ألا يتجاوز 100 حرف.")]
        public string JobTitle { get; set; } = null!;

        [Required(ErrorMessage = "الموظف المرتبط بالخبرة مطلوب.")]
        public string EmployeeId { get; set; } = null!;

        public Employee Employee { get; set; } = null!;
    }
}
