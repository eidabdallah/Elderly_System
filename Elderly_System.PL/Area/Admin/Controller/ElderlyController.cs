using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Request.Room;
using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ElderlyController : ControllerBase
    {
        private readonly IElderlyAdminService _service;

        public ElderlyController(IElderlyAdminService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetElderlies([FromQuery] Status? status)
        {
            var result = await _service.GetElderliesAsync(status);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromQuery] Status status)
        {
            var result = await _service.ChangeElderlyStatusAsync(id, status);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetElderlyDetails([FromRoute] int id)
        {
            var result = await _service.GetElderlyDetailsAsync(id);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
        [HttpPost("stay")]
        public async Task<IActionResult> AddStay([FromBody] AddResidentStayRequest req)
        {
            var result = await _service.AddResidentStayAsync(req);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpGet("rooms/available")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            var result = await _service.GetAvailableRoomsAsync();

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
        [HttpGet("by-stay")]
        public async Task<IActionResult> GetElderliesByStay([FromQuery] StayFilter? filter)
        {
            var result = await _service.GetElderliesByStayAsync(filter);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
        [HttpGet("{elderlyId}/rooms/available-for-change")]
        public async Task<IActionResult> GetRoomsForChange([FromRoute] int elderlyId)
        {
            var result = await _service.GetAvailableRoomsForChangeAsync(elderlyId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPatch("{elderlyId}/stay/change-room")]
        public async Task<IActionResult> ChangeRoom([FromRoute] int elderlyId, [FromBody] ChangeRoomRequest req)
        {
            var result = await _service.ChangeResidentRoomAsync(elderlyId, req.NewRoomId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpPatch("{elderlyId}/stay/end")]
        public async Task<IActionResult> EndStay([FromRoute] int elderlyId)
        {
            var result = await _service.EndResidentStayAsync(elderlyId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
