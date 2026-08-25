using Microsoft.AspNetCore.Identity;
using SecureAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SecureAuth.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> AddRoleAsync(User user, string role);
        Task<IdentityResult> CreatePasswordAsync(User user, string password);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveExpiredTokens(User user);
        Task<User?> GetByRefreshTokenAsync(string token);
        Task RevokeAllRefreshToken(User user);
        Task RevokeRefreshTokenAsync(string token);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
    }
}
