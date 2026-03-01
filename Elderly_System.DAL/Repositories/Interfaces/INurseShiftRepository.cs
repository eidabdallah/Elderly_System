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
        Task<int> SaveChangesAsync();
    }
}
