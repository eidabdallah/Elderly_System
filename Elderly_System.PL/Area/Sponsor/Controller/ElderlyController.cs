using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Sponsor.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Sponsor")]
    [Authorize(Roles = "Sponsor")]
    public class ElderlyController : ControllerBase
    {
        private readonly IElderlySponsorService _service;

        public ElderlyController(IElderlySponsorService service)
        {
            _service = service;
        }
        [HttpPost("add-with-doctor")]
        public async Task<IActionResult> AddWithDoctor([FromForm] AddElderlyWithDoctorRequest request)
        {
            var sponsorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(sponsorId))
                return Unauthorized(new { message = "غير مصرح." });

            var result = await _service.AddElderlyWithDoctorAsync(sponsorId, request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpPost("verify-link")]
        public async Task<IActionResult> VerifyLink([FromBody] VerifyElderlySponsorLinkRequest request)
        {
            var result = await _service.VerifyLinkAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
