namespace Elderly_System.DAL.DTO.Response.Medicine
{
    public class ElderlyWeeklyMedicationRowDto
    {
        public int DrugPlanId { get; set; }
        public string MedicineName { get; set; } = "";
        public string MedicineTypeName { get; set; } = "";
        public int DailyIntake { get; set; }
        public List<string> ScheduledTimes { get; set; } = new(); 
        public Dictionary<string, ElderlyMedicationDayCellDto> Days { get; set; } = new();
    }
}
