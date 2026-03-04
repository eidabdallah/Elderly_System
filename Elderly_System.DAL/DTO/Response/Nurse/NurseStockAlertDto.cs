namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseStockAlertDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";

        public int DrugPlanId { get; set; }
        public string MedicineName { get; set; } = "";

        public int StockStatus { get; set; } 
        public string StockStatusText { get; set; } = ""; 
        public string Message { get; set; } = "";
    }
}
