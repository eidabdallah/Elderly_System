using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class CheckListService : ICheckListService
    {
        private readonly ICheckListRepository _repository;

        public CheckListService(ICheckListRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> AddCheckListAsync(AddCheckListRequest request, string nurseId)
        {
            if (request.ElderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            var elderly = await _repository.GetElderlyByIdAsync(request.ElderlyId);
            if (elderly == null)
                return ServiceResult.Failure("المسن غير موجود.");

            var nurse = await _repository.GetNurseByIdAsync(nurseId);
            if (nurse == null)
                return ServiceResult.Failure("الممرضة غير موجودة.");

            var checkList = new CheckList
            {
                ElderlyId = request.ElderlyId ,
                NurseId = nurseId,
                Notes = request.Notes ?? "-",
                Temperature = request.Temperature ?? "-",
                Pulse = request.Pulse ?? "-",
                BloodSugar = request.BloodSugar ?? "-",
                BloodPressure = request.BloodPressure ?? "-",
                Intake = request.Intake ?? "-",
                Output = request.Output ?? "-",
                DateTime = DateTime.Now
            };

            await _repository.AddCheckListAsync(checkList);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تمت إضافة الفحص اليومي بنجاح.");
        }

        public async Task<ServiceResult> GetCheckListsByElderlyIdAsync(int elderlyId)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            var elderly = await _repository.GetElderlyByIdAsync(elderlyId);
            if (elderly == null)
                return ServiceResult.Failure("المسن غير موجود.");

            var data = await _repository.GetCheckListsByElderlyIdAsync(elderlyId);

            return ServiceResult.SuccessWithData(data, "تم جلب الفحوصات بنجاح.");
        }

        public async Task<ServiceResult> GetCheckListByIdAsync(int checkListId)
        {
            if (checkListId <= 0)
                return ServiceResult.Failure("رقم الفحص غير صحيح.");

            var checkList = await _repository.GetCheckListByIdAsync(checkListId);
            if (checkList == null)
                return ServiceResult.Failure("الفحص غير موجود.");

            var nurseId = checkList.Nurse?.Id;
            if (string.IsNullOrWhiteSpace(nurseId))
                return ServiceResult.Failure("لا يوجد ممرضة مرتبطة بهذا الفحص.");

            var shiftKey = await _repository.GetNurseShiftKeyByDateAsync(nurseId, checkList.DateTime);

            var dto = new CheckListResponse
            {
                CheckListId = checkList.Id,
                ElderlyId = checkList.ElderlyId,
                ElderlyName = checkList.Elderly.Name,
                NurseName = checkList.Nurse.FullName,
                Time = checkList.DateTime.ToString("HH:mm"),
                Shift = shiftKey ?? "-",
                Notes = checkList.Notes ?? "-",
            };

            return ServiceResult.SuccessWithData(dto, "تم جلب الفحص بنجاح.");
        }

        public async Task<ServiceResult> UpdateCheckListAsync(int checkListId, UpdateCheckListRequest request)
        {
            if (checkListId <= 0)
                return ServiceResult.Failure("رقم الفحص غير صحيح.");

            var checkList = await _repository.GetCheckListByIdAsync(checkListId);
            if (checkList == null)
                return ServiceResult.Failure("الفحص غير موجود.");

            checkList.Notes = request.Notes;
            checkList.Temperature = request.Temperature;
            checkList.Pulse = request.Pulse;
            checkList.BloodSugar = request.BloodSugar;
            checkList.BloodPressure = request.BloodPressure;
            checkList.Intake = request.Intake;
            checkList.Output = request.Output;

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم تعديل الفحص بنجاح.");
        }

        public async Task<ServiceResult> DeleteCheckListAsync(int checkListId)
        {
            if (checkListId <= 0)
                return ServiceResult.Failure("رقم الفحص غير صحيح.");

            var checkList = await _repository.GetCheckListByIdAsync(checkListId);
            if (checkList == null)
                return ServiceResult.Failure("الفحص غير موجود.");

            await _repository.DeleteCheckListAsync(checkList);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessMessage("تم حذف الفحص بنجاح.");
        }
    }
}
