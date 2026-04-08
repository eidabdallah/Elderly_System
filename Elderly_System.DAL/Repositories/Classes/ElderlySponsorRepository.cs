using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Sponsor;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderlySponsorRepository : IElderlySponsorRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderlySponsorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SponsorElderlyBriefDto?> GetMyElderliesAsync(string sponsorId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Where(e => e.ElderlySponsors.Any(es => es.SponsorId == sponsorId))
                .Select(e => new SponsorElderlyBriefDto
                {
                    ElderlyId = e.Id,
                    ElderlyName = e.Name,
                    Sponsor = e.ElderlySponsors
                        .Where(es => es.SponsorId == sponsorId)
                        .Select(es => new SponsorRelationDto
                        {
                            SponsorId = es.SponsorId,
                            SponsorName = es.Sponsor.FullName ?? "",
                            KinShip = es.KinShip,
                            Degree = es.Degree
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<Elderly?> GetByIdFullDetailsForSponsorAsync(string sponsorId)
        {
            return await _context.Elderlies
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Where(e => e.ElderlySponsors.Any(es => es.SponsorId == sponsorId))
                 .Include(e => e.ResidentStays)
                     .ThenInclude(s => s.Room)
                         .ThenInclude(r => r.RoomImages)
                 .Include(e => e.MedicalReports)
                     .ThenInclude(r => r.Doctor)
                 .FirstOrDefaultAsync();
        }
        public async Task<MedicalReport?> GetMedicalReportByIdAsync(int reportId)
        {
            return await _context.MedicalReports
                .AsNoTracking()
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == reportId);
        }
        public async Task<List<SponsorElderlyMedicinesResponse>> GetMyElderliesMedicinesAsync(string sponsorId)
        {
            var today = DateTime.Today;

            var rows = await _context.DrugPlans
                .AsNoTracking()
                .Where(dp =>
                    dp.Elderly.ElderlySponsors.Any(es => es.SponsorId == sponsorId)
                )
                .Where(dp => dp.MedicineStatus == Status.Active)
                .Select(dp => new
                {
                    dp.ElderlyId,
                    ElderlyName = dp.Elderly.Name,
                    MedicineName = dp.Medicine.Name,
                    MedicineType = dp.Medicine.Type,
                    dp.Status,
                    dp.EndDate
                })
                .OrderBy(r => r.ElderlyName)
                .ToListAsync();

            var result = rows
                .GroupBy(r => new { r.ElderlyId, r.ElderlyName })
                .Select(g => new SponsorElderlyMedicinesResponse
                {
                    Medicines = g.Select(x => new ElderlyMedicineResponse
                    {
                        MedicineName = x.MedicineName,
                        MedicineTypeName = x.MedicineType switch
                        {
                            MedicineType.Tablet => "حبوب",
                            MedicineType.Syrup => "سائل",
                            _ => "غير محدد"
                        },                       
                        StatusTypeName = x.Status switch { 
                            Status.Active =>"متبقي",
                            Status.InActive => "قريب الانتهاء",
                            Status.Finish => "منتهي",
                            _ => "غير محدد"
                        },
                    })
                    .ToList()
                })
                .ToList();

            return result;
        }
        public async Task<List<SponsorElderlyTodayChecklistsResponse>> GetMyElderliesTodayChecklistsAsync(string sponsorId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var rows = await _context.CheckLists
                .AsNoTracking()
                .Where(c =>
                    c.DateTime >= today && c.DateTime < tomorrow &&
                    c.Elderly.ElderlySponsors.Any(es => es.SponsorId == sponsorId)
                )
                .Select(c => new
                {
                    c.ElderlyId,
                    ElderlyName = c.Elderly.Name,
                    Item = new SponsorChecklistItemResponse
                    {
                        Id = c.Id,
                        NurseId = c.NurseId,
                        NurseName = c.Nurse.FullName, 
                        Notes = c.Notes,
                        Temperature = c.Temperature,
                        Pulse = c.Pulse,
                        BloodSugar = c.BloodSugar,
                        BloodPressure = c.BloodPressure,
                        Intake = c.Intake,
                        Output = c.Output
                    }
                })
                .ToListAsync();

            var result = rows
                .GroupBy(x => new { x.ElderlyId, x.ElderlyName })
                .Select(g => new SponsorElderlyTodayChecklistsResponse
                {
                    ElderlyId = g.Key.ElderlyId,
                    ElderlyName = g.Key.ElderlyName,
                    CheckLists = g.Select(x => x.Item).ToList()
                })
                .OrderBy(x => x.ElderlyName)
                .ToList();

            return result;
        }
        public async Task<Elderly?> GetElderlyWithCurrentDoctorBySponsorIdAsync(string sponsorId)
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Where(e => e.ElderlySponsors.Any(es => es.SponsorId == sponsorId))
                .Include(e => e.Doctor)
                    .ThenInclude(d => d.WorkPlaces)
                .FirstOrDefaultAsync();
        }
        public async Task<List<DoctorNameResponse>> GetAvailableDoctorsForSponsorAsync(string sponsorId)
        {
            var currentDoctorId = await _context.Elderlies
                .AsNoTracking()
                .Where(e => e.ElderlySponsors.Any(es => es.SponsorId == sponsorId))
                .Select(e => e.DoctorId)
                .FirstOrDefaultAsync();

            var doctorsQuery = _context.Users
                .AsNoTracking()
                .OfType<Doctor>();

            if (!string.IsNullOrWhiteSpace(currentDoctorId))
            {
                doctorsQuery = doctorsQuery.Where(d => d.Id != currentDoctorId);
            }

            return await doctorsQuery
                .OrderBy(d => d.FullName)
                .Select(d => new DoctorNameResponse
                {
                    DoctorId = d.Id,
                    Name = d.FullName ?? ""
                })
                .ToListAsync();
        }
        public async Task<Elderly?> GetElderlyBySponsorIdAsync(string sponsorId)
        {
            return await _context.Elderlies
                .FirstOrDefaultAsync(e => e.ElderlySponsors.Any(es => es.SponsorId == sponsorId));
        }

        public async Task<bool> DoctorExistsAsync(string doctorId)
        {
            return await _context.Users
                .OfType<Doctor>()
                .AnyAsync(d => d.Id == doctorId);
        }

        public async Task<bool> HasPendingDoctorChangeRequestAsync(int elderlyId)
        {
            return await _context.DoctorChangeRequests
                .AnyAsync(r => r.ElderlyId == elderlyId && r.RequestStatus == Status.Pending);
        }

        public async Task AddDoctorChangeRequestAsync(DoctorChangeRequest request)
        {
            await _context.DoctorChangeRequests.AddAsync(request);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
