using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطبيب مطلوب.")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "مكان عمل الطبيب مطلوب.")]
        [StringLength(150, ErrorMessage = "مكان العمل يجب ألا يتجاوز 150 حرف.")]
        public string WorkPlace { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string Phone { get; set; } = null!;

        public ICollection<MedicalReport> MedicalReports { get; set; } = new List<MedicalReport>();
    }
}
