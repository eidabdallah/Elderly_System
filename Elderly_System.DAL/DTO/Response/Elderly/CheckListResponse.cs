namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class CheckListResponse
    {
        public int CheckListId { get; set; }
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = null!;
        public string NurseName { get; set; } = null!;
        public string Shift { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
