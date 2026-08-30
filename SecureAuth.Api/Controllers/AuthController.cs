using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureAuth.Api.Attributes;
using SecureAuth.Application.DTOs;
using SecureAuth.Application.Interfaces.Services;
using System.Security.Claims;

namespace SecureAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseController
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            //Console.WriteLine($"Registering user: {registerDto.UserName}, Email: {registerDto.Email}");
            var result = await _authService.RegisterAsync(registerDto);
            return HandleResult(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            //Console.WriteLine($"Logging in user: {loginDto.UserName}");
            var result = await _authService.LoginAsync(loginDto);
            return HandleResult(result);
        }
        [HttpPost("resend")]
        public async Task<IActionResult> Resend([FromBody] EmailDto resendDto)
        {
            var result = await _authService.ResendOtpAsync(resendDto);
            return HandleResult(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyOtpDto verifyOtpDto)
        {
            var result = await _authService.VerifyOtpAsync(verifyOtpDto);
            return HandleResult(result);
        }

        [HttpPost("send/reset")]
        public async Task<IActionResult> SendReset([FromBody] EmailDto sendResetDto)
        {
            var result = await _authService.SendRestPasswordOtpAsync(sendResetDto);
            return HandleResult(result);
        }

        [HttpPost("verify/reset")]
        public async Task<IActionResult> VerifyReset([FromBody] VerifyOtpDto verifyOtpDto)
        {
            var result = await _authService.VerifyRestPasswordOtpAsync(verifyOtpDto);
            return HandleResult(result);
        }
        [HasPermission("user.resetPassword")]
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await _authService.ResetPasswordAsync(resetPasswordDto , email!);
            return HandleResult(result);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken!);
            return HandleResult(result);
        }


        [HasPermission("user.read")]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.LogoutAsync(refreshToken!);
            return HandleResult(result);
        }

        [HasPermission("user.read")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authService.GetMe(email!);
            return HandleResult(result);
        }


    }
}
