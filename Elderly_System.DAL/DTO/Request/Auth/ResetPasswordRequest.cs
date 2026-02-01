using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.DTO.Request.Auth
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,100}$",
           ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير وحرف صغير ورقم ورمز (مثل !@#).")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "الكود مطلوب.")]
        public string Code { get; set; }
    }
}
