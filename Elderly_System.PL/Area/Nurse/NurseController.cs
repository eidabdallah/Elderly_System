using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[area]/[controller]")]
    [Area("Nurse")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class NurseController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly INurseShiftService _shiftService;

        public NurseController(IUserService service , INurseShiftService shiftService)
        {
            _service = service;
            _shiftService = shiftService;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetDetails()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return Unauthorized("لم يتم العثور على بيانات المستخدم داخل التوكن. يرجى تسجيل الدخول مرة أخرى.");
            }
            var result = await _service.GetUserDetailsAsync(id);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, User = result.Data });
        }
        [HttpGet("my-schedule")]
        public async Task<IActionResult> GetMySchedule([FromQuery] int offset = 0)
        {
            var nurseId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(nurseId))
                return Unauthorized("لم يتم العثور على بيانات المستخدم داخل التوكن. يرجى تسجيل الدخول مرة أخرى.");

            var result = await _shiftService.GetMyWeeklyScheduleAsync(nurseId, offset);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
