using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Sponsor;
using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlySponsorRepository
    {
        Task<SponsorElderlyBriefDto?> GetMyElderliesAsync(string sponsorId);
        Task<Elderly?> GetByIdFullDetailsForSponsorAsync(string sponsorId);
        Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId);
        Task<List<SponsorElderlyMedicinesResponse>> GetMyElderliesMedicinesAsync(string sponsorId);
        Task<List<SponsorElderlyTodayChecklistsResponse>> GetMyElderliesTodayChecklistsAsync(string sponsorId);
        Task<Elderly?> GetElderlyWithCurrentDoctorBySponsorIdAsync(string sponsorId);
        Task<List<DoctorNameResponse>> GetAvailableDoctorsForSponsorAsync(string sponsorId);
        Task<Elderly?> GetElderlyBySponsorIdAsync(string sponsorId);
        Task<bool> DoctorExistsAsync(string doctorId);
        Task<bool> HasPendingDoctorChangeRequestAsync(int elderlyId);
        Task AddDoctorChangeRequestAsync(DoctorChangeRequest request);
        Task SaveChangesAsync();
    }
}
