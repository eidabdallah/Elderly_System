namespace Elderly_System.DAL.DTO.Response.Medicine
{
    public class ElderlyMedicationDayCellDto
    {
        public bool InPlanPeriod { get; set; }
        public int TakenCount { get; set; }
        public int RequiredCount { get; set; }
        public List<MedicationTakenDto> Taken { get; set; } = new();
        public string Summary { get; set; } = "";
    }
}
