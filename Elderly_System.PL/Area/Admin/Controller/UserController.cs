using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUsers([FromQuery] Status? status, [FromQuery] Role? role)
        {
            var result = await _service.GetUsersAsync(status, role);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, Users = result.Data });
        }
    }
}
