namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class CheckListListResponse
    {
        public int CheckListId { get; set; }
        public string DateTime { get; set; } = null!;
        public string Temperature { get; set; } = null!;
        public string Pulse { get; set; } = null!;
        public string BloodSugar { get; set; } = null!;
        public string BloodPressure { get; set; } = null!;
    }
}
