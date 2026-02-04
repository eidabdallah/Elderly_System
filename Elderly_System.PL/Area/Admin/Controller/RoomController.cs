using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddRoom([FromForm] RoomCreateRequest request)
        {
            var result = await _service.AddRoomAsync(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
    }
}
