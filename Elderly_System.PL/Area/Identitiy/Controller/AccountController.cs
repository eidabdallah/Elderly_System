using Elderly_System.BLL.Service.Authentication;
using ElderlySystem.DAL.DTO.Request.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Identitiy.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Identity")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request , Request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var ok = await _authenticationService.ConfirmEmailAsync(token, userId);
            var page = ok == "email confirmed successfully"
                ? "/pages/confirm-success.html"
                : "/pages/confirm-failed.html";

            return Redirect(page);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            if (result.Data != null)
                return Ok(new { message = result.Message, token = result.Data });
            return Ok(new { message = result.Message });
        }

    }
}
