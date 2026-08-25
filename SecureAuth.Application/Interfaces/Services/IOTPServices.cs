using SecureAuth.Application.Common.Result;
using SecureAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Interfaces.Services
{
    public interface IOTPService
    {
        Task<Result> SendEmailOtp(User user);
        Task<Result> VerifyEmailOtp(User user, string code);
    }
}
