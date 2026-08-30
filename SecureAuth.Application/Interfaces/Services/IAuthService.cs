using SecureAuth.Application.Common.Result;
using SecureAuth.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result> RegisterAsync(RegisterDto registerDto);
        public Task<Result> ResendOtpAsync(EmailDto emailDto);
        public Task<Result<AuthResponse>> VerifyOtpAsync(VerifyOtpDto verifyOtpDto);
        public Task<Result<AuthResponse>> LoginAsync(LoginDto loginDto);
        Task<Result> SendRestPasswordOtpAsync(EmailDto emailDto);
        Task<Result<TokenDto>> VerifyRestPasswordOtpAsync(VerifyOtpDto verifyOtpDto);
        Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto , string email);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
        Task<Result> LogoutAsync(string refreshToken);
        Task<Result<UserDto>> GetMe(string email);
    }
}
