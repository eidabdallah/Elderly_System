namespace Elderly_System.DAL.DTO.Response.Statistics
{
    public class AdminDashboardDto
    {
        public AdminDashboardStatsDto Stats { get; set; } = new();
        public List<DonationMonthDto> DonationsOverTime { get; set; } = new();
    }
}
