using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.DTO.Request.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        public string Password { get; set; } = default!;
    }
}
