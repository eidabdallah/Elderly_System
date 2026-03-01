namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseShiftScheduleRowDto
    {
        public string NurseId { get; set; } = null!;
        public string NurseName { get; set; } = "";
        public Dictionary<string, string> Days { get; set; } = new();
    }
}
