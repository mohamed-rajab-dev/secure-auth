

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecureAuth.Application.Interfaces.Repositories;
using SecureAuth.Domain.Entities;
using SecureAuth.Infrastructure.Persistence;
using System.Security.Claims;

namespace SecureAuth.Infrastructure.Repositories
{
    public class UserRepository(UserManager<User> userManager, AppDbContext context) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _context = context;


        public async Task<User?> GetByIdAsync(long id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task AddAsync(User user)
        {
            await _userManager.CreateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.Set<RefreshToken>().AddAsync(refreshToken);

            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.Set<RefreshToken>().FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task RemoveExpiredTokens(User user)
        {
            await _context.Set<RefreshToken>()
                .Where(rt => rt.UserId == user.Id &&
                             rt.ExpiresAt < DateTime.UtcNow)
                .ExecuteDeleteAsync();
        }

        public async Task RevokeAllRefreshToken(User user)
        {
            await _context.Set<RefreshToken>()
                .Where(rt => rt.UserId == user.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(rt => rt.RevokedAt, DateTimeOffset.UtcNow));

        }

        public async Task<IdentityResult> UpdatePasswordAsync(User user, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> CreatePasswordAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }


        public async Task<IdentityResult> AddRoleAsync(User user, string role)
        {
            return  await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<User?> GetByRefreshTokenAsync(string token)
        {
            return await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == token));
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            await _context.Set<RefreshToken>()
                .Where(rt => rt.Token == token)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(rt => rt.RevokedAt, DateTimeOffset.UtcNow));
        }

        public async Task<List<Claim>> GetPermissionsFromUserRoles(User user)
        {
            var permissions = await (
            from userRole in _context.Set<IdentityUserRole<long>>()
            join rolePermission in _context.Set<IdentityRoleClaim<long>>()
                on userRole.RoleId equals rolePermission.RoleId
            where userRole.UserId == user.Id && rolePermission.ClaimType == "permission"
            select new Claim("permission", rolePermission.ClaimValue!)
            ).Distinct().ToListAsync();

            return permissions;
        }
    }
}
