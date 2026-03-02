using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class ElderMealMiniDto
    {
        public int ElderlyId { get; set; }
        public DateTime Date { get; set; }
        public MealType MealType { get; set; }
        public string MealDetails { get; set; } = "";
    }
}
