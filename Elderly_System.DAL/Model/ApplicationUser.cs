using Elderly_System.DAL.Enums;
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
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "اسم المدينة مطلوب.")]
        public string City { get; set; } = null!;
        public string? CodeResetPassword { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }

        [RegularExpression(@"^\d{9}$", ErrorMessage = "يجب ان يكون رقم الهوية مكون من 9 ارقام")]
        public string NationalId { get; set; } = null!;
        [Required(ErrorMessage = "الجنس مطلوب.")]
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pending;
        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string PhoneNumberForValidation => PhoneNumber ?? string.Empty;

        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();


    }
}
