using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.DTOs
{
    public class UserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
