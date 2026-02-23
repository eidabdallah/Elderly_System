using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Http;
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

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يكون مكوّن من 10 أرقام فقط.")]
        public string PhoneNumber { get; set; } = default!;


        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يكون مكونًا من 9 أرقام.")]
        public string NationalId { get; set; } = default!;

        [Required(ErrorMessage = "المدينة مطلوبة.")]
        [MaxLength(100, ErrorMessage = "اسم المدينة طويل جدًا.")]
        public string City { get; set; } = default!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "الجنس مطلوب.")]
        public Gender GenderSponsor { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "درجة الكفيل مطلوب.")]
        public SponsorDegree SponsorDegree { get; set; }



        public string? Name { get; set; }

        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يكون مكونًا من 9 أرقام.")]
        public string NationalIdElderly { get; set; } = default!;
        public string? Doctrine { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string? CityElderly { get; set; }
        public string? Street { get; set; }
        public string? HealthStatus { get; set; }
        public DateTime? BDate { get; set; }
        public string? ReasonRegister { get; set; }
        public List<string>? Diseases { get; set; }
        public IFormFile? NationalIdImage { get; set; }
        public IFormFile? HealthInsurance { get; set; }
        [Required(ErrorMessage = "صلة القرابة مطلوبة.")]
        public string KinShip { get; set; } = default!;
        [Required(ErrorMessage = "الدرجة مطلوبة.")]
        public string Degree { get; set; } = default!;
        public string? DoctorName { get; set; }
        public string? WorkPlace { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string? DoctorPhone { get; set; }
        public DateTime? ReportDate { get; set; }
        public IFormFile? DiagnosisFile { get; set; }

    }
}
