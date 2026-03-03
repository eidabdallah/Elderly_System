using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.Medicine
{
    public class ElderlyDrugPlanResponse
    {
        public int DrugPlanId { get; set; }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = null!;
        public string MedicineTypeName { get; set; } = null!;

        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;

        public int DailyIntake { get; set; }
        public string? Notes { get; set; }
        public Status Status { get; set; }
        public Status MedicineStatus { get; set; }
        public List<string> Times { get; set; } = new();
    }
}
