using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Area("Admin")]
    [Route("api/[area]/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;

        public DashboardController(IAdminDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _service.GetDashboardAsync();

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
