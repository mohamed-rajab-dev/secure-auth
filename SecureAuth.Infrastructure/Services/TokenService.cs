using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureAuth.Application.Interfaces.Repositories;
using SecureAuth.Application.Interfaces.Services;
using SecureAuth.Domain.Entities;
using SecureAuth.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SecureAuth.Infrastructure.Services
{
    public class TokenService(UserManager<User> userManager, IOptions<Jwt> jwt, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : ITokenService
    {
        private readonly UserManager<User> userManager = userManager;
        private readonly IUserRepository userRepository = userRepository;
        private readonly Jwt jwt = jwt.Value;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        private async Task<string> JwtToken(User user, IEnumerable<Claim> additionalClaims , DateTime? expires = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(additionalClaims),
                Expires = expires ?? DateTime.UtcNow.AddMinutes(jwt.DurationInMinute),
                SigningCredentials = creds,
                Issuer = jwt.Issuer,
                Audience = jwt.Audience
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);

        }

        public async Task<string> CreateJwtToken(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var permissions = await userRepository.GetPermissionsFromUserRoles(user);

            foreach (var permission in permissions) {
                Console.WriteLine($"{permission.Value}");
            }

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            claims.AddRange(userClaims);
            claims.AddRange(permissions);
            claims.AddRange(roles.Select(role => new Claim("role", role)));

            return await JwtToken(user, claims);
        }

        public async Task<string> CreateJwtTokenWithPermissions(User user, DateTime? expires = null, params string[] permissions)
        {
            var claims = permissions.Select(permission => new Claim("permission", permission)).ToList();

            claims.AddRange(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            });

            return await JwtToken(user, claims, expires);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken(Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddDays(10));
        }

        //public async Task<RefreshToken?> RefreshJwtToken(string refreshToken)
        //{
        //    var token = await userRepository.GetRefreshToken(refreshToken);
        //    if (token == null || !token.IsActive)
        //    {
        //        return null;
        //    }
        //    token.Revoke();
        //    await userRepository.UpdateRefreshToken(token);
        //    return token;
        //}

        public void SetRefreshTokenCookie(string refreshToken, DateTimeOffset expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict

            };
            httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public void DeleteRefreshTokenCookie()
        {
            httpContextAccessor.HttpContext!.Response.Cookies.Delete("refreshToken");
        }

        public string? GetRefreshTokenFromCookie()
        {
            return httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];
        }
    }
}
