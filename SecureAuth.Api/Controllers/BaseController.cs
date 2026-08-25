using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureAuth.Application.Common.Result;

namespace SecureAuth.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult(
            Result result,
            int successStatusCode = StatusCodes.Status200OK)
        {
            if (!result.Success)
                return BadRequest(result);

            return StatusCode(successStatusCode, result);
        }
    }
}
