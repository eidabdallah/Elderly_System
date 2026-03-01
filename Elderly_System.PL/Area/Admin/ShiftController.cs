using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Nurse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ShiftController : ControllerBase
    {
        private readonly INurseShiftService _service;

        public ShiftController(INurseShiftService service)
        {
            _service = service;
        }
        [HttpGet("active-nurses")]
        public async Task<IActionResult> GetActiveNurses()
        {
            var result = await _service.GetActiveNursesAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, nurses = result.Data });
        }

        [HttpPost("assign-daily")]
        public async Task<IActionResult> AssignDaily([FromBody] AssignDailyShiftsRequest request)
        {
            var result = await _service.AssignDailyShiftsAsync(request);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
