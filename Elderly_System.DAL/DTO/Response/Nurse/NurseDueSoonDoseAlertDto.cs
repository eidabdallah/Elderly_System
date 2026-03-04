namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseDueSoonDoseAlertDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";

        public int DrugPlanId { get; set; }
        public string MedicineName { get; set; } = "";

        public string DueTime { get; set; } = "";    
        public int MinutesLeft { get; set; }          
        public string Message { get; set; } = "";
        public string ReminderKey { get; set; } = "";
    }
}
