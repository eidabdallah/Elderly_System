using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Vistor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VisitorController : ControllerBase
    {
        private readonly IVistorService _service;

        public VisitorController(IVistorService service)
        {
            _service = service;
        }
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _service.GetPendingRequestsAsync();

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPatch("reply/{requestId}")]
        public async Task<IActionResult> Reply([FromRoute] int requestId, [FromBody] AdminVisitorReplyRequest request)
        {
            var result = await _service.ReplyAsync(requestId, request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
