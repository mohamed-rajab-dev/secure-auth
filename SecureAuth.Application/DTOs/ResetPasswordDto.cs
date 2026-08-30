using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.DTOs
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }
}
