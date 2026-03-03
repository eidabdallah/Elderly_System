using Elderly_System.DAL.DTO.Request.Medicine;
using Elderly_System.DAL.DTO.Response.Medicine;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IMedicineService
    {
        Task<ServiceResult> AddDrugPlanAsync(AddDrugPlanRequest request);
        Task<List<MedicineLookupResponse>> GetAllMedicinesAsync(string? search, int? type);
        Task<List<ElderlyDrugPlanResponse>?> GetElderlyDrugPlansAsync(int elderlyId);

        Task<ServiceResult> UpdateDrugPlanAsync(int drugPlanId, DrugPlanUpdateRequest request);
        Task<ServiceResult> UpdateDrugPlanStatusAsync(int drugPlanId, int status);
        Task<ServiceResult> UpdateDrugPlanStatusMedAsync(int drugPlanId, int status);
        Task<ServiceResult> AddMedicationAsync(MedicationCreateRequest request, string nurseId);
        Task<List<ElderlyDrugPlanResponse>?> GetElderlyMedicineAsync(int elderlyId);
        Task<ServiceResult> GetElderlyWeeklyMedicationScheduleAsync(int elderlyId, int offset = 0);



    }
}
