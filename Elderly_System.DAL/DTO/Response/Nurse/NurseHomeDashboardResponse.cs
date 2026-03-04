namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseHomeDashboardResponse
    {
        public int ActiveElderliesCount { get; set; }

        public NurseWeeklyShiftDto MyWeeklyShift { get; set; } = new();

        public string TodayShiftKey { get; set; } = "-";
        public List<NurseMiniDto> TodayTeam { get; set; } = new();

        public List<NurseOverdueDoseAlertDto> OverdueDoses { get; set; } = new();
        public List<NursePlanExpiringAlertDto> PlansExpiringSoon { get; set; } = new();
        public List<NurseStockAlertDto> LowOrOutStock { get; set; } = new();

        public List<NurseTodayActivityDto> TodayActivity { get; set; } = new();
    }
}
