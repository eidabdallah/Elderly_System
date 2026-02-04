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
            var AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(AdminId))
                return Unauthorized(new { message = "تعذر تحديد المستخدم من التوكن." });
            var result = await _service.CreateDonationAsync(request, AdminId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteDonationAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }
        /*[HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DonationUpdateRequest request)
        {
            var result = await _service.UpdateDonationAsync(id, request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }*/

    }
}
