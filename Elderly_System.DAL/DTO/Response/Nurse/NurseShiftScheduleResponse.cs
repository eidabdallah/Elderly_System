namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseShiftScheduleResponse
    {
        public List<string> Dates { get; set; } = new(); 
        public List<NurseShiftScheduleRowDto> Rows { get; set; } = new();
    }
}
