using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface ICheckListRepository
    {
        Task<Elderly?> GetElderlyByIdAsync(int elderlyId);
        Task<Nurse?> GetNurseByIdAsync(string nurseId);

        Task AddCheckListAsync(CheckList checkList);
        Task<CheckList?> GetCheckListByIdAsync(int checkListId);
        Task<List<CheckListListResponse>> GetCheckListsByElderlyIdAsync(int elderlyId);
        Task<string?> GetNurseShiftKeyByDateAsync(string nurseId, DateTime date);

        Task SaveChangesAsync();
        Task DeleteCheckListAsync(CheckList checkList);
    }
}
