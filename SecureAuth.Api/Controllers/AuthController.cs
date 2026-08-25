using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureAuth.Application.DTOs;
using SecureAuth.Application.Interfaces.Services;

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

    }
}
