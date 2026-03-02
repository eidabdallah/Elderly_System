using Elderly_System.DAL.Enums;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    [Index(nameof(ElderlyId), nameof(Date), nameof(MealType), IsUnique = true)]
    public class ElderMeal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;

        [Required(ErrorMessage = "التاريخ مطلوب.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "نوع الوجبة مطلوب.")]
        [StringLength(30, ErrorMessage = "نوع الوجبة يجب ألا يتجاوز 30 حرف.")]
        public MealType MealType { get; set; }

        [Required(ErrorMessage = "اسم الوجبة مطلوب.")]
        [StringLength(30, ErrorMessage = "اسم الوجبة يجب ألا يتجاوز 30 حرف.")]
        public string MealDetails { get; set; } = null!;
    }
}
