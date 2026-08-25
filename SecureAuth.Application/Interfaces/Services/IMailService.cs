using Microsoft.AspNetCore.Http;
using SecureAuth.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task<Result> SendEmail(string mailTo, string subject, string body, IList<IFormFile>? attachments = null);
    }
}
