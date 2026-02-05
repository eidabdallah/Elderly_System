using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class RegisterCoSponsorRequest
    {
        [Required(ErrorMessage = "صلة القرابة مطلوبة.")]
        public string KinShip { get; set; } = null!;

        [Required(ErrorMessage = "درجة القرابة مطلوبة.")]
        public string Degree { get; set; } = null!;

        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "اسم المستخدم مطلوب.")]
        [MaxLength(50, ErrorMessage = "اسم المستخدم طويل جدًا.")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح.")]
        public string Email { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "الجنس مطلوب.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "المدينة مطلوبة.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يتكون من 9 أرقام.")]
        public string NationalId { get; set; } = null!;

        public string? Note { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,100}$",
           ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير وحرف صغير ورقم ورمز (مثل !@#).")]
        public string Password { get; set; } = null!;
    }
}
