namespace Elderly_System.DAL.DTO.Response.Medicine
{
    public class ElderlyWeeklyMedicationScheduleResponse
    {
        public List<string> Dates { get; set; } = new();
        public List<ElderlyWeeklyMedicationRowDto> Rows { get; set; } = new();
    }
}
