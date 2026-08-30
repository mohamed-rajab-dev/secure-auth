using SecureAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
