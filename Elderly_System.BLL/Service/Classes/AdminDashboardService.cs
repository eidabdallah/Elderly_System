using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Response.Statistics;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.BLL.Service.Classes
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repo;

        public AdminDashboardService(IAdminDashboardRepository repo)
        {
            _repo = repo;
        }
        public async Task<ServiceResult> GetStatsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var dto = new AdminDashboardStatsDto
            {
                ElderlyCount = await _repo.CountElderliesAsync(),
                SponsorsCount = await _repo.CountUsersInRoleAsync("Sponsor"),
                NursesCount = await _repo.CountUsersInRoleAsync("Nurse"),
                DonationsCountToDate = await _repo.CountDonationsToDateAsync(today),
                EventsCountToDate = await _repo.CountEventsToDateAsync(today),
            };
            return ServiceResult.SuccessWithData(dto , "تم جلب الإحصائيات بنجاح");

        }
    }
}
