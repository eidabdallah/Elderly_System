using Elderly_System.BLL.Service.Interface;
using ElderlySystem.DAL.DTO.Request.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authenticationService.ForgotPasswordAsync(request);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> AuthMe()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserId))
                return Unauthorized();
            var result = await _authenticationService.AuthMeAsync(UserId!);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
