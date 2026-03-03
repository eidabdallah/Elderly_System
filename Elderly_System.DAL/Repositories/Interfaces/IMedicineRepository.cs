using Elderly_System.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IMedicineRepository
    {
        Task<bool> ElderlyExistsAsync(int elderlyId);
        Task<Medicine?> GetMedicineByIdAsync(int id);
        Task<Medicine?> GetMedicineByNameAndTypeAsync(string name, Enums.MedicineType type);
        Task AddMedicineAsync(Medicine medicine);
        Task AddDrugPlanAsync(DrugPlan drugPlan);
        Task<List<Medicine>> GetAllMedicinesAsync(string? search, int? type);
        Task<List<DrugPlan>> GetDrugPlansByElderlyIdAsync(int elderlyId);
        Task<DrugPlan?> GetDrugPlanWithTimesAsync(int drugPlanId);
        Task UpdateDrugPlanAsync(DrugPlan drugPlan);
        Task ReplaceDrugPlanTimesAsync(int drugPlanId, List<DrugPlanTime> newTimes);
    }
}
