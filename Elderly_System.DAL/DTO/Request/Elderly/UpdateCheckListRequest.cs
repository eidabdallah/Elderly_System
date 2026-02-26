using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class UpdateCheckListRequest
    {
        public string? Notes { get; set; }
        public string? Temperature { get; set; }
        public string? Pulse { get; set; }
        public string? BloodSugar { get; set; }
        public string? BloodPressure { get; set; }
        public string? Intake { get; set; }
        public string? Output { get; set; }
    }
}
