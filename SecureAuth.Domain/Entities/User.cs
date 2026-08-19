using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Domain.Entities
{
    public class User : IdentityUser<long>
    {
    }
}
