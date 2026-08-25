using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Domain.Entities
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public void ConfirmEmail()
        {
            if (EmailConfirmed)
                return;

            EmailConfirmed = true;
        }

        public bool isEmailConfirmed => EmailConfirmed;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Add(refreshToken);
        }

        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Remove(refreshToken);
        }

        public void RemoveAllRefreshTokens()
        {
            RefreshTokens.Clear();
        }
    }
}
