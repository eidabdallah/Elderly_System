using EderlySystem.DAL.Enums;
using Elderly_System.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    [Index(nameof(NationalId), IsUnique = true)]
    public class Elderly
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المسن مطلوب.")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "يجب أن يكون رقم الهوية مكوّنًا من 9 أرقام.")]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "الديانة / المذهب مطلوب.")]
        [StringLength(50, ErrorMessage = "القيمة يجب ألا تتجاوز 50 حرف.")]
        public string Doctrine { get; set; } = null!;

        [Required(ErrorMessage = "الحالة الاجتماعية مطلوبة.")]
        public MaritalStatus MaritalStatus { get; set; }

        [Required(ErrorMessage = "اسم المدينة مطلوب.")]
        [StringLength(50, ErrorMessage = "اسم المدينة يجب ألا يتجاوز 50 حرف.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "اسم الشارع مطلوب.")]
        [StringLength(100, ErrorMessage = "اسم الشارع يجب ألا يتجاوز 100 حرف.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "الحالة الصحية مطلوبة.")]
        [StringLength(200, ErrorMessage = "الحالة الصحية يجب ألا تتجاوز 200 حرف.")]
        public string HealthStatus { get; set; } = null!;
        public ICollection<string> Diseases { get; set; } = new List<string>();

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب.")]
        public DateTime BDate { get; set; }

        [Required(ErrorMessage = "صورة الفحص الشامل مطلوبة.")]
        public string ComprehensiveExamination { get; set; } = null!;

        [Required(ErrorMessage = "صورة الهوية مطلوبة.")]
        public string NationalIdImage { get; set; } = null!;

        [Required(ErrorMessage = "صورة التأمين الصحي مطلوبة.")]
        public string HealthInsurance { get; set; } = null!;

        [Required(ErrorMessage = "سبب التسجيل مطلوب.")]
        [StringLength(300, ErrorMessage = "السبب يجب ألا يتجاوز 300 حرف.")]
        public string ReasonRegister { get; set; } = null!;

        public Status status { get; set; } = Status.Pending;


        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BDate.Year;
                if (BDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public ICollection<ElderlySponsor> ElderlySponsors { get; set; } = new List<ElderlySponsor>();
        public ICollection<ResidentStay> ResidentStays { get; set; } = new List<ResidentStay>();
        public ICollection<ElderlyVisitor> ElderlyVisitors { get; set; } = new List<ElderlyVisitor>();
        public ICollection<ElderMeal> ElderMeals { get; set; } = new List<ElderMeal>();
        public ICollection<MedicalReport> MedicalReports { get; set; } = new List<MedicalReport>();
        public ICollection<DrugPlan> DrugPlans { get; set; } = new List<DrugPlan>();


    }
}
