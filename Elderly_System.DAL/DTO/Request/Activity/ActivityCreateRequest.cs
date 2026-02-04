using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Activity
{
    public class ActivityCreateRequest
    {
        [Required(ErrorMessage = "اسم النشاط مطلوب")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "اسم النشاط يجب أن يكون بين 3 و 150 حرف.")]
        public string ActivityName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "موقع النشاط مطلوب.")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "تاريخ النشاط مطلوب")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "وقت بدء النشاط مطلوب.")]
        public TimeSpan StartTime { get; set; }

        [MinLength(1, ErrorMessage = "يجب إضافة جهة منظمة أو مشارك واحد على الأقل.")]
        public List<ParticipantCreateRequest> ActivityOrganizations { get; set; } = new();
    }
}
