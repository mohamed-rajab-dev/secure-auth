using Microsoft.EntityFrameworkCore;
using SecureAuth.Application.Interfaces.Repositories;
using SecureAuth.Domain.Entities;
using SecureAuth.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.Set<RefreshToken>().FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.Set<RefreshToken>().Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
