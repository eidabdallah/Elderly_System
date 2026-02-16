using Elderly_System.DAL.Enums;
using ElderlySystem.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetUsersAsync(Status? status = null, string? name = null);
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<ApplicationUser?> GetBaseAsync(string id);
        Task<Employee?> GetEmployeeAsync(string id);
        Task<Nurse?> GetNurseAsync(string id);
        Task<Sponsor?> GetSponsorWithElderlyAsync(string id);
        Task<Nurse?> GetByIdAsync(string id);
        Task UpdateAsync(Nurse nurse);



    }
}
