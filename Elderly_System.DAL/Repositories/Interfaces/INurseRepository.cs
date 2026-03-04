using Elderly_System.DAL.Enums;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface INurseRepository
    {
        Task<int> CountActiveElderliesAsync();

        Task<List<NurseShiftAssignment>> GetAssignmentsInRangeAsync(DateTime startDate, DateTime endDate);
        Task<NurseShiftAssignment?> GetNurseAssignmentByDateAsync(string nurseId, DateTime date);
        Task<List<NurseShiftAssignment>> GetAssignmentsByShiftAndDateAsync(int shiftId, DateTime date);

        Task<List<(int DrugPlanId, int ElderlyId, string ElderlyName, string MedicineName, DateTime EndDate, Status StockStatus)>>
            GetTodayActivePlansAsync(DateTime today);

        Task<List<(int DrugPlanId, TimeSpan Time)>> GetPlanTimesAsync(List<int> drugPlanIds);

        Task<Dictionary<int, int>> GetTodayMedicationCountsByPlanAsync(List<int> drugPlanIds, DateTime start, DateTime end);

        Task<List<(DateTime DateTime, int DrugPlanId, int ElderlyId, string ElderlyName,  string MedicineName, string Dose, string NurseId, string NurseName)>>
            GetTodayMedicationActivityAsync(DateTime start, DateTime end, int take);

        Task<Dictionary<string, string>> GetShiftKeysForNursesOnDateAsync(List<string> nurseIds, DateTime date);
    }
}
