using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.DTOs
{
    public class AuthResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token {  get; set; } = string.Empty;

    }
}
