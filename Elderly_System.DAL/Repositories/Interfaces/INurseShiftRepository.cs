using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface INurseShiftRepository
    {
        Task<List<Nurse>> GetActiveNursesAsync();

        Task<List<Nurse>> GetActiveNursesByIdsAsync(List<string> nurseIds);
        Task<List<NurseShiftAssignment>> GetAssignmentsByDateAndNurseIdsAsync(DateTime date, List<string> nurseIds);

        Task<Dictionary<char, Shift>> GetShiftsByKeysAsync(List<char> keys);

        Task AddAssignmentsAsync(List<NurseShiftAssignment> assignments);
        Task<List<NurseShiftAssignment>> GetAssignmentsInRangeAsync(DateTime start, DateTime end);
        Task<Nurse?> GetActiveNurseByIdAsync(string nurseId);

        Task<List<NurseShiftAssignment>> GetAssignmentsInRangeByNurseAsync(string nurseId, DateTime start, DateTime end);

        Task<List<DateTime>> GetScheduledDaysInRangeAsync(DateTime start, DateTime end);

        Task<int> SaveChangesAsync();
    }
}
