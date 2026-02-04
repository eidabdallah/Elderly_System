using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElderlySystem.DAL.DTO.Request.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        [MaxLength(256, ErrorMessage = "البريد الإلكتروني طويل جدًا.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,100}$",
            ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير وحرف صغير ورقم ورمز (مثل !@#).")]
        public string Password { get; set; } = default!;


        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        [MaxLength(200, ErrorMessage = "الاسم الكامل طويل جدًا.")]
        public string FullName { get; set; } = default!;

        [Required(ErrorMessage = "اسم المستخدم مطلوب.")]
        [MaxLength(50, ErrorMessage = "اسم المستخدم طويل جدًا.")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^(059|056)\d{7}$", ErrorMessage = "رقم الهاتف يجب أن يكون 10 أرقام ويبدأ بـ 059 أو 056.")]
        public string PhoneNumber { get; set; } = default!;


        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يكون مكونًا من 9 أرقام.")]
        public string NationalId { get; set; } = default!;

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة.")]
        [MaxLength(100, ErrorMessage = "اسم المدينة طويل جدًا.")]
        public string City { get; set; } = default!;

        [MaxLength(100, ErrorMessage = "اسم الشارع طويل جدًا.")]
        public string? Street { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "الجنس مطلوب.")]
        public Gender Gender { get; set; }

        [MaxLength(500, ErrorMessage = "الملاحظة طويلة جدًا.")]
        public string? Note { get; set; }
    }
}
