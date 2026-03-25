using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Auth
{
    public class RegisterDoctorRequest
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "يجب ان يكون رقم الهوية مكون من 9 ارقام")]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "المدينة مطلوبة.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "الجنس مطلوب.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public Gender GenderDoctor { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "الرتبة الطبية مطلوبة.")]
        public string MedicalRank { get; set; } = null!;

        [Required(ErrorMessage = "سنوات الخبرة مطلوبة.")]
        public string YearsOfExperience { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "عدد العمليات غير صحيح.")]
        public int NumberOfOperations { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب.")]
        public DateTime BDate { get; set; }

        public List<string>? Specializations { get; set; }
        public List<string>? diseasesDoctor { get; set; }
        public List<string>? WorkPlaces { get; set; }
        public List<string>? OperationTypes { get; set; }
        public List<string>? MedicalProcedures { get; set; }
        public List<string>? DiagnosticTests { get; set; }
        public List<string>? PreviousWorkPlaces { get; set; }

        public List<DoctorUniversityRequest>? Universities { get; set; }
    }
}
