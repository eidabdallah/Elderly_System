using Elderly_System.DAL.DTO.Response.Nurse;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlyNurseRepository
    {
        Task<List<NurseElderlyListDto>> GetActiveResidentElderliesAsync();
    }
}
