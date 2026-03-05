using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.Sponsor
{
    public class ElderlyMedicineResponse
    {
        public string MedicineName { get; set; } = null!;
        public string MedicineTypeName { get; set; } = null!;
        public string StatusTypeName { get; set; } = null!;
    }
}
