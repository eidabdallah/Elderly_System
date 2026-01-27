using EderlySystem.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    [Index(nameof(NationalId), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]

    public class ApplicationUser :IdentityUser
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "اسم المدينة مطلوب.")]
        public string City { get; set; }
        public string? Street { get; set; }
        public string? CodeResetPassword { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }
        [Required(ErrorMessage = "تاريخ الميلاد مطلوب.")]

        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "رقم الهوية مطلوب.")]

        [RegularExpression(@"^\d{9}$", ErrorMessage = "يجب ان يكون رقم الهوية مكون من 9 ارقام")]
        public string NationalId { get; set; }
        [Required(ErrorMessage = "الجنس مطلوب.")]
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pending;
    }
}
