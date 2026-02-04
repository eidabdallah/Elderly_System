using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _service.GetAllActivitiesAsync();
            return Ok(new { message = "تم جلب الأنشطة بنجاح", data = activities });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _service.GetActivityByIdAsync(id);
            if (activity == null)
                return NotFound(new { message = "النشاط غير موجود." });

            return Ok(new { message = "تم جلب النشاط بنجاح", data = activity });
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ActivityCreateRequest request)
        {
            var AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(AdminId))
                return Unauthorized(new { message = "تعذر تحديد المستخدم من التوكن." });

            var result = await _service.CreateActivityAsync(request, AdminId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteActivityAsync(id);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
