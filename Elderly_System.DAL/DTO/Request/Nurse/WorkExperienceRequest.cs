using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Nurse
{
    public class WorkExperienceRequest
    {
        [Required(ErrorMessage = "اسم مكان العمل مطلوب")]
        [StringLength(100, ErrorMessage = "اسم مكان العمل يجب ألا يزيد عن 100 حرف")]
        public string WorkName { get; set; } = null!;

        [Required(ErrorMessage = "موقع العمل مطلوب")]
        [StringLength(100, ErrorMessage = "موقع العمل يجب ألا يزيد عن 100 حرف")]
        public string WorkLocation { get; set; } = null!;

        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        [StringLength(50, ErrorMessage = "المسمى الوظيفي يجب ألا يزيد عن 50 حرف")]
        public string JobTitle { get; set; } = null!;
    }
}
