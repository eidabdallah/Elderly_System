using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Activity
{
    public class ParticipantCreateRequest
    {
        [Required(ErrorMessage = "اسم الجهة المشاركة مطلوب.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "اسم الجهة يجب أن يكون بين 2 و 150 حرف.")]
        public string OrganizationName { get; set; } = null!;
    }
}
