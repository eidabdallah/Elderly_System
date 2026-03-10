using Elderly_System.DAL.DTO.Response.Sponsor;
using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IElderlySponsorRepository
    {
        Task<List<SponsorElderlyBriefDto>> GetMyElderliesWithAllSponsorsAsync(string sponsorId);
        Task<Elderly?> GetByIdFullDetailsForSponsorAsync(string sponsorId);
        Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId);
        Task<List<SponsorElderlyMedicinesResponse>> GetMyElderliesMedicinesAsync(string sponsorId);
        Task<List<SponsorElderlyTodayChecklistsResponse>> GetMyElderliesTodayChecklistsAsync(string sponsorId);
    }
}
