using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Donation : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المتبرع مطلوب.")]
        [StringLength(100, ErrorMessage = "اسم المتبرع يجب ألا يتجاوز 100 حرف.")]
        public string DonorName { get; set; } = null!;

        [Required(ErrorMessage = "تاريخ التبرع مطلوب.")]
        public DateTime DonationDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "نوع التبرع مطلوب.")]
        public DonationType DonationType { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999", ErrorMessage = "قيمة التبرع يجب أن تكون أكبر من صفر.")]
        public decimal? MonetaryAmount { get; set; }

        [StringLength(3, ErrorMessage = "العملة يجب ألا تتجاوز 3 أحرف.")]
        public string? Currency { get; set; }

        [Required(ErrorMessage = "الموظف المسؤول مطلوب.")]
        public string EmployeeId { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public ICollection<Good> Goods { get; set; } = new List<Good>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DonationType == DonationType.Cash)
            {
                if (MonetaryAmount is null || MonetaryAmount <= 0)
                    yield return new ValidationResult("قيمة التبرع النقدي مطلوبة ويجب أن تكون أكبر من صفر.", new[] { nameof(MonetaryAmount) });

                if (string.IsNullOrWhiteSpace(Currency))
                    yield return new ValidationResult("العملة مطلوبة للتبرع النقدي.", new[] { nameof(Currency) });

                if (Goods.Count > 0)
                    yield return new ValidationResult("التبرع النقدي لا يجب أن يحتوي على عناصر عينية.", new[] { nameof(Goods) });
            }
            else
            {
                if (Goods.Count == 0)
                    yield return new ValidationResult("يجب إضافة عنصر واحد على الأقل في التبرع العيني.", new[] { nameof(Goods) });

                if (MonetaryAmount != null)
                    yield return new ValidationResult("التبرع العيني لا يجب أن يحتوي على مبلغ نقدي.", new[] { nameof(MonetaryAmount) });

                if (!string.IsNullOrWhiteSpace(Currency))
                    yield return new ValidationResult("التبرع العيني لا يحتاج عملة.", new[] { nameof(Currency) });
            }
        }
    }
}
