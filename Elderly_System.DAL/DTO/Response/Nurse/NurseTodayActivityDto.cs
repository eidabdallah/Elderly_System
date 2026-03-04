namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseTodayActivityDto
    {
        public string Time { get; set; } = "";
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";

        public int DrugPlanId { get; set; }
        public string MedicineName { get; set; } = "";
        public string Dose { get; set; } = "";

        public string NurseId { get; set; } = "";
        public string NurseName { get; set; } = "";
        public string ShiftKey { get; set; } = "-";

        public string Action { get; set; } = "تم تسجيل جرعة";
    }
}
