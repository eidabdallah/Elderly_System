namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class CheckListListResponse
    {
        public int CheckListId { get; set; }
        public string Date { get; set; } = null!;
        public string? Temperature { get; set; }
        public string? Pulse { get; set; }
        public string? BloodSugar { get; set; }
        public string? BloodPressure { get; set; }
        public string? Intake { get; set; }
        public string? Output { get; set; }
    }
}
