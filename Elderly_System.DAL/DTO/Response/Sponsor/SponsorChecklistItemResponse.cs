namespace Elderly_System.DAL.DTO.Response.Sponsor
{
    public class SponsorChecklistItemResponse
    {
        public int Id { get; set; }
        public string NurseId { get; set; } = "";
        public string NurseName { get; set; } = "";

        public string? Notes { get; set; }
        public string? Temperature { get; set; }
        public string? Pulse { get; set; }
        public string? BloodSugar { get; set; }
        public string? BloodPressure { get; set; }
        public string? Intake { get; set; }
        public string? Output { get; set; }
    }
}
