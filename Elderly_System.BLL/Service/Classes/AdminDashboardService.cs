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
        public async Task<ServiceResult> GetDashboardAsync()
        {
            var dto = new AdminDashboardDto
            {
                Stats = new AdminDashboardStatsDto
                {
                    ElderliesCount = await _repo.CountElderliesAsync(),
                    SponsorsCount = await _repo.CountSponsorAsync(),
                    NursesCount = await _repo.CountUsersInRoleAsync("Nurse"),
                    DonationsCount = await _repo.CountDonationsAsync(),
                    ActivitiesCount = await _repo.CountActivitiesAsync(),
                    RoomsCount = await _repo.CountRoomsAsync()
                },
                DonationsOverTime = await _repo.GetDonationsOverTimeAsync()
            };

            return ServiceResult.SuccessWithData(dto, "تم جلب بيانات لوحة التحكم بنجاح");
        }
    }
}
