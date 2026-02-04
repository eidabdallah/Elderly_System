using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Donation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _service;

        public DonationController(IDonationService service)
        {
            _service = service;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] DonationCreateRequest request)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateDonationAsync(request, UserId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
