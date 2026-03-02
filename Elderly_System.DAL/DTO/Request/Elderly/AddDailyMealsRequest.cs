using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class AddDailyMealsRequest
    {
        [Required(ErrorMessage = "رقم المسن مطلوب.")]
        public int ElderlyId { get; set; }

        [Required(ErrorMessage = "التاريخ مطلوب.")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "اكلة الفطور مطلوبة.")]
        [StringLength(250)]
        public string Breakfast { get; set; } = "";

        [Required(ErrorMessage = "اكلة الغداء مطلوبة.")]
        [StringLength(250)]
        public string Lunch { get; set; } = "";

        [Required(ErrorMessage = "اكلة العشاء مطلوبة.")]
        [StringLength(250)]
        public string Dinner { get; set; } = "";
    }
}
