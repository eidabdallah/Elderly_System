using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.DTO.Request.Auth
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; } = default!;
    }
}
