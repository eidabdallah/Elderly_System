using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Model
{
    [PrimaryKey(nameof(MedicalReportId), nameof(MedicineId))]
    public class MedicalReportMedicine
    {
        public int MedicalReportId { get; set; }
        public MedicalReport MedicalReport { get; set; } = null!;

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = null!;
    }
}
