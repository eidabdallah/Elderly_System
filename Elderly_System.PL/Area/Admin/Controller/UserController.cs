using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Nurse;
using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] string id)
        {
            var result = await _service.GetUserDetailsAsync(id);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, User = result.Data });
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUsers([FromQuery] Status? status, [FromQuery] Role? role , [FromQuery] string? name)
        {
            var result = await _service.GetUsersAsync(status, role , name);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, Users = result.Data });
        }
        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] string id, [FromQuery] Status status)
        {
            var result = await _service.ChangeStatusAsync(id, status);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpPatch("change-role/{id}")]
        public async Task<IActionResult> ChangeRole([FromRoute] string id, [FromQuery] Role role)
        {
            var result = await _service.ChangeRoleAsync(id, role);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
