using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Nurse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class NurseController : ControllerBase
    {
        private readonly IUserService _service;

        public NurseController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteProfile([FromBody] CompleteNurseProfileRequest request)
        {
            var nurseId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(nurseId))
                return Unauthorized(new { message = "رقم المستخدم غير موجود بالتوكن" });

            var result = await _service.CompleteProfileAsync(nurseId, request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
