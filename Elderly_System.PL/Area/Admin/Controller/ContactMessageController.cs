using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.ContactMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {
        private readonly IContactMessageService _service;

        public ContactMessageController(IContactMessageService service)
        {
            _service = service;
        }
        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMessages()
        {
            var result = await _service.GetAdminMessagesAsync();
            return Ok(new { message = result.Message, data = result.Data });
        }
        [HttpPost("{id}/reply")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reply([FromRoute] int id, [FromBody] ReplyContactMessageRequest request)
        {
            var result = await _service.ReplyAsync(id, request);
            if (!result.Success) return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpPost("msgUser")]
        public async Task<IActionResult> AddMessage([FromBody] AddContactMessageRequest request)
        {
            var result = await _service.AddAsync(request);
            if (!result.Success) return BadRequest(new {message  = result.Message});
            return Ok(new { message = result.Message });

        }
    }
}
