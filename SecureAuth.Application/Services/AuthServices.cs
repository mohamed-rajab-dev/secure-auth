using FluentValidation;
using MapsterMapper;
using SecureAuth.Application.Common.Result;
using SecureAuth.Application.DTOs;
using SecureAuth.Application.Interfaces.Repositories;
using SecureAuth.Application.Interfaces.Services;
using SecureAuth.Domain.Entities;


namespace SecureAuth.Application.Services
{
    public class AuthServices(IUserRepository userRepository, ITokenService tokenService, IOTPService otpService, IMapper mapper, IRefreshTokenRepository refreshTokenRepository) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IOTPService _otpService = otpService;
        private readonly IMapper _mapper = mapper;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        public async Task<Result> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto is null)
                return Result.FailureResult("Invalid registration data.");

            var user = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    var resendOtpResult = await _otpService.SendEmailOtp(user);

                    if (!resendOtpResult.Success)
                    {
                        // logging the error message from resendOtpResult.Message
                        return Result.SuccessResult(
                            "If registration is successful, a verification code will be sent to your email."
                        );
                    }

                    return Result.SuccessResult(
                        "If registration is successful, a verification code will be sent to your email."
                    );

                }

                return Result.FailureResult(
                    "Email is already registered."
                );
            }

            user = _mapper.Map<User>(registerDto);

            var userCreate = await _userRepository.CreatePasswordAsync(user, registerDto.Password);

            await _userRepository.AddRoleAsync(user, "User");

            //if (!userCreate.Succeeded)
            //{
            //    return Result<object>.FailureResult(
            //        "User creation failed.",
            //        "Password",
            //        userCreate.Errors
            //            .Select(e => e.Description)
            //            .ToArray()
            //    );
            //}

            await _otpService.SendEmailOtp(user);

            return Result.SuccessResult(
                "If registration is successful, a verification code will be sent to your email."
            );

        }
        public async Task<Result> ResendOtpAsync(EmailDto emailDto)
        {
            if (emailDto is null)
                return Result.FailureResult("Invalid email data.");
            var user = await _userRepository.GetByEmailAsync(emailDto.Email);

            var resendResult = await _otpService.SendEmailOtp(user!);

            if (!resendResult.Success)
            {
                // logging the error message from resendResult.Message
                return Result.SuccessResult("If the email address is available, you will receive a verification code (OTP).");
            }

            return Result.SuccessResult(
                "If the email address is available, you will receive a verification code (OTP)."
            );
        }
        public async Task<Result<AuthResponse>> VerifyOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            if (verifyOtpDto is null)
                return Result<AuthResponse>.FailureResult("Invalid verification data.");
            var user = await _userRepository.GetByEmailAsync(verifyOtpDto.Email);

            if (user is null)
                return Result<AuthResponse>.FailureResult(
                    "The verification request could not be completed."
                );

            var verifyResult = await _otpService.VerifyEmailOtp(user, verifyOtpDto.Code);

            if (!verifyResult.Success)
                return Result<AuthResponse>.FailureResult(verifyResult.Message);

            var token = await _tokenService.CreateJwtToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();


            user.AddRefreshToken(refreshToken);

            await _userRepository.UpdateAsync(user);

            _tokenService.SetRefreshTokenCookie(refreshToken.Token, DateTimeOffset.Now.AddDays(7));

            return Result<AuthResponse>.SuccessResult(
                "Your email has been successfully verified.", new AuthResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Token = token,
                }
            );

        }
        public async Task<Result<AuthResponse>> LoginAsync(LoginDto loginDto)
        {
            if (loginDto is null)
                return Result<AuthResponse>.FailureResult("Invalid login data.");

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user is null)
                return Result<AuthResponse>.FailureResult(
                    "Invalid email or password."
                );

            var passwordValid = await _userRepository.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordValid)
                return Result<AuthResponse>.FailureResult(
                    "Invalid email or password."
                );

            if (!user.EmailConfirmed)
                return Result<AuthResponse>.FailureResult(
                    "Email is not verified. Please verify your email before logging in."
                );
            var token = await _tokenService.CreateJwtToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RemoveAllRefreshTokens();

            user.AddRefreshToken(refreshToken);

            await _userRepository.UpdateAsync(user);

            _tokenService.SetRefreshTokenCookie(refreshToken.Token, DateTimeOffset.Now.AddDays(7));
            return Result<AuthResponse>.SuccessResult(
                "Login successful.", new AuthResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Token = token,
                }
            );
        }

        public async Task<Result> SendRestPasswordOtpAsync(EmailDto emailDto)
        {
            if (emailDto is null)
                return Result.FailureResult("Invalid email data.");

            var user = await _userRepository.GetByEmailAsync(emailDto.Email);
            if (user is null)
                return Result.SuccessResult(
                    "If the email address is available, you will receive a verification code (OTP)."
                );
            var sendOtpResult = await _otpService.SendRestPassword(user);

            if (!sendOtpResult.Success)
            {
                // logging the error message from sendOtpResult.Message
                return Result.SuccessResult("If the email address is available, you will receive a verification code (OTP).");
            }

            return Result.SuccessResult(
                "If the email address is available, you will receive a verification code (OTP)."
            );

        }

        public async Task<Result<TokenDto>> VerifyRestPasswordOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            if (verifyOtpDto is null)
                return Result<TokenDto>.FailureResult("Invalid verification data.");

            var user = await _userRepository.GetByEmailAsync(verifyOtpDto.Email);
            if (user is null)
                return Result<TokenDto>.FailureResult("User not found.");

            var verifyResult = await _otpService.VerifyRestPassword(user, verifyOtpDto.Code);

            if (!verifyResult.Success)
                return Result<TokenDto>.FailureResult(verifyResult.Message);

            var token = await _tokenService.CreateJwtTokenWithPermissions(user, DateTime.Now.AddMinutes(5), "user.resetPassword");

            return Result<TokenDto>.SuccessResult("OTP verified successfully.", new TokenDto { Token = token });
        }
        public async Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto, string email)
        {
            if (resetPasswordDto is null)
                return Result.FailureResult("Invalid reset password data.");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                return Result.FailureResult("User not found.");

            var resetResult = await _userRepository.UpdatePasswordAsync(user, resetPasswordDto.NewPassword);

            if (!resetResult.Succeeded)
                return Result.FailureResult("Password reset failed.",
                    "NewPassword",
                    resetResult.Errors
                        .Select(e => e.Description)
                        .ToArray()
                );
            return Result.SuccessResult("Password has been successfully reset.");
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
            if (token is null)
                return Result<AuthResponse>.FailureResult("Failed to refresh token.");

            if(token.IsExpired || !token.IsActive)
                return Result<AuthResponse>.FailureResult("Refresh token is expired or inactive.");

            var user = await _userRepository.GetByIdAsync(token.UserId);

            //if (user is null)
            //    return Result.FailureResult("User not found.");

            token.Revoke();
            await _refreshTokenRepository.UpdateRefreshTokenAsync(token);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _tokenService.SetRefreshTokenCookie(newRefreshToken.Token, DateTimeOffset.Now.AddDays(7));

            user!.AddRefreshToken(newRefreshToken);
            await _userRepository.UpdateAsync(user);

            var newJwtToken = await _tokenService.CreateJwtToken(user);

            return Result<AuthResponse>.SuccessResult(
                "Token refreshed successfully.", new AuthResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Token = newJwtToken,
                }
            );
        }

        public async Task<Result> LogoutAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
            if (token is null)
                return Result.FailureResult("Failed to logout.");

            if (token.IsExpired || !token.IsActive)
                return Result.FailureResult("Refresh token is expired or inactive.");

            token.Revoke();
            await _refreshTokenRepository.UpdateRefreshTokenAsync(token);
            _tokenService.DeleteRefreshTokenCookie();
            return Result.SuccessResult("Logout successful.");
        }

        public async Task<Result<UserDto>> GetMe(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                return Result<UserDto>.FailureResult("User not found.");

            return Result<UserDto>.SuccessResult("User retrieved successfully.", new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                UserId = user.Id,
                UserName = user.UserName!
            });
        }

    }
}
