using Elderly_System.DAL.DTO.Request.Nurse;
using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Auth
{
    public class RegisterStaffRequest
    {
        [Required] public string FullName { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")] 
        public string PhoneNumber { get; set; } = null!;
        [Required] public string City { get; set; } = null!;
        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يتكون من 9 أرقام.")]
        public string NationalId { get; set; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required] public Gender Gender { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,100}$",
           ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير وحرف صغير ورقم ورمز (مثل !@#).")]
        public string Password { get; set; } = null!;
        public IFormFile Certificate { get; set; } = null!;

        [Required(ErrorMessage = "المستوى التعليمي مطلوب")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationLevel EducationLevel { get; set; }

        [Required(ErrorMessage = "الحالة الاجتماعية مطلوبة")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaritalStatus MaritalStatus { get; set; }

        [Required(ErrorMessage = "مجال الدراسة مطلوب")]
        [StringLength(100, ErrorMessage = "مجال الدراسة يجب ألا يزيد عن 100 حرف")]
        public string FieldOfStudy { get; set; } = null!;

        [Range(0, 20, ErrorMessage = "عدد سنوات الدراسة يجب أن يكون بين 0 و 20")]
        public float YearsOfStudy { get; set; }

        [Required(ErrorMessage = "سنة التخرج مطلوبة")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "سنة التخرج يجب أن تكون 4 أرقام مثل 2022")]
        public string YearOfGraduation { get; set; } = null!;
        public List<WorkExperienceRequest>? WorkExperiences { get; set; } = new List<WorkExperienceRequest>();
    }
}
