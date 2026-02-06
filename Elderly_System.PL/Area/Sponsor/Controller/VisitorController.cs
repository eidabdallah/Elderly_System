using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Vistor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Sponsor.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Sponsor")]
    [Authorize(Roles = "Sponsor")]
    public class VisitorController : ControllerBase
    {
        private readonly IVistorService _service;

        public VisitorController(IVistorService service)
        {
            _service = service;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddVisitor([FromBody] AddVisitorRequest request)
        {
            var sponsorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.AddVisitorAsync(sponsorId!, request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}