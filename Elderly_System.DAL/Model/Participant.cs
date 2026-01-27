using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Participant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "اسم الجهة المشاركة مطلوب.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "اسم الجهة يجب أن يكون بين 2 و 150 حرف.")]
        public string OrganizationName { get; set; } = null!;

        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
    }
}
