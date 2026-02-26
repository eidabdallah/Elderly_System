using Elderly_System.DAL.DTO.Request.Elderly;
using ElderlySystem.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.BLL.Service.Interface
{
    public interface ICheckListService
    {
        Task<ServiceResult> AddCheckListAsync(AddCheckListRequest request, string nurseId);
        Task<ServiceResult> GetCheckListsByElderlyIdAsync(int elderlyId);
        Task<ServiceResult> GetCheckListByIdAsync(int checkListId);
        Task<ServiceResult> UpdateCheckListAsync(int checkListId, UpdateCheckListRequest request);
        Task<ServiceResult> DeleteCheckListAsync(int checkListId);
    }
}
