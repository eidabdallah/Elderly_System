namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NursePlanExpiringAlertDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";
        public string RoomNumber { get; set; } = "";

        public int DrugPlanId { get; set; }
        public string MedicineName { get; set; } = "";

        public string EndDate { get; set; } = ""; // yyyy-MM-dd
        public int DaysLeft { get; set; }
        public string Message { get; set; } = "";
    }
}
