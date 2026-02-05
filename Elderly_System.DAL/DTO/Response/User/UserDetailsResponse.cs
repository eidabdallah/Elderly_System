using Elderly_System.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Response.User
{
    public class UserDetailsResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string City { get; set; } = "";
        public string? Street { get; set; }
        public string NationalId { get; set; } = "";
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }

        public string RoleUser { get; set; } = "";

        public string? JobTitle { get; set; }
        public string? HireDate { get; set; }
        public string? EducationLevel { get; set; }
        public string? FieldOfStudy { get; set; }
        public float? YearsOfStudy { get; set; }
        public string? AcademicDegree { get; set; }
        public string? YearDfGraduation { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Skills { get; set; }

        public string? ImageCertificate { get; set; }

        public string? Note { get; set; }
        public List<string>? ElderlyNames { get; set; }

        public static string ToArabic(Role role) => role switch
        {
            Role.Admin => "أدمن",
            Role.Employee => "موظف",
            Role.Nurse => "ممرض",
            Role.Sponsor => "كفيل",
            _ => "غير معروف"
        };
        public static string ToArabic(Gender g) => g switch
        {
            Enums.Gender.Male => "ذكر",
            Enums.Gender.Female => "أنثى",
            _ => "غير محدد"
        };
        public static string ToArabic(Status s) => s switch
        {
            Enums.Status.Pending => "انتظار القبول",
            Enums.Status.Active => "نشط",
            Enums.Status.InActive => "غير نشط",
            _ => "غير معروف"
        };
        public static string ToArabic(EducationLevel e) => e switch
        {
            Enums.EducationLevel.Secondary => "ثانوي",
            Enums.EducationLevel.Tawjihi => "توجيهي",
            Enums.EducationLevel.University => "دبلوم",
            Enums.EducationLevel.Institute => "جامعة",
            _ => "غير معروف"
        };
        public static string ToArabic(MaritalStatus m) => m switch
        {
            Enums.MaritalStatus.Single => "أعزب/عزباء",
            Enums.MaritalStatus.Married => "متزوج/ة",
            Enums.MaritalStatus.Divorced => "مطلق/ة",
            Enums.MaritalStatus.Widowed => "أرمل/ة",
            _ => "غير معروف"
        };




    }
}
