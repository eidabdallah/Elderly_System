namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseOverdueDoseAlertDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";
        public string RoomNumber { get; set; } = "";

        public int DrugPlanId { get; set; }
        public string MedicineName { get; set; } = "";

        public string DueTime { get; set; } = "";     
        public int LateMinutes { get; set; }          
        public string Message { get; set; } = "";     
    }
}
