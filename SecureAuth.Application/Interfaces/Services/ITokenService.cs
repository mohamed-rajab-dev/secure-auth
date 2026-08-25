using SecureAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateJwtToken(User user);
        RefreshToken GenerateRefreshToken();
        void SetRefreshTokenCookie(string refreshToken, DateTimeOffset expires);
        string? GetRefreshTokenFromCookie();
        void DeleteRefreshTokenCookie();
    }
}
