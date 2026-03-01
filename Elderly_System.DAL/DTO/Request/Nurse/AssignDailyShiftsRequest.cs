namespace Elderly_System.DAL.DTO.Request.Nurse
{
    public class AssignDailyShiftsRequest
    {
        public DateTime Date { get; set; }

        public List<string> ANurseIds { get; set; } = new();
        public List<string> BNurseIds { get; set; } = new();
        public List<string> CNurseIds { get; set; } = new();
    }
}
