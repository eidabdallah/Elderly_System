using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.DTO.Request.Auth;
using Microsoft.AspNetCore.Http;

namespace Elderly_System.BLL.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResult> RegisterAsync(RegisterRequest request, HttpRequest HttpRequest);
        Task<ServiceResult> LoginAsync(LoginRequest request);
        Task<string> ConfirmEmailAsync(string token, string userId);
        Task<ServiceResult> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        //Task<ServiceResult> AuthMeAsync(string userId);
    }
}
