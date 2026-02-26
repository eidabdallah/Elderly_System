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

        public string Notes { get; set; } = null!;
        public string Temperature { get; set; } = null!;
        public string Pulse { get; set; } = null!;
        public string BloodSugar { get; set; } = null!;
        public string BloodPressure { get; set; } = null!;
        public string Intake { get; set; } = null!;
        public string Output { get; set; } = null!;
        public string InOut { get; set; } = null!;
    }
}
