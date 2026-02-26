namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class CheckListResponse
    {
        public int CheckListId { get; set; }
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = null!;
        public string NurseId { get; set; } = null!;
        public string NurseName { get; set; } = null!;
        public string DateTime { get; set; } = null!;

        public string? Notes { get; set; }
        public string? Temperature { get; set; }
        public string? Pulse { get; set; }
        public string? BloodSugar { get; set; }
        public string? BloodPressure { get; set; }
        public string? Intake { get; set; }
        public string? Output { get; set; }
    }
}
