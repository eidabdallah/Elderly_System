namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class WeeklyElderMealRowDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";
        public Dictionary<string, DailyMealCellDto> Days { get; set; } = new();
    }
}
