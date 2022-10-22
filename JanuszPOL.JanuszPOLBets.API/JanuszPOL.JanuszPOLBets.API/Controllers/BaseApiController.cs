using Microsoft.AspNetCore.Authorization;
using JanuszPOL.JanuszPOLBets.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace JanuszPOL.JanuszPOLBets.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult<ServiceResult<T>>> MethodWrapper<T>(Func<Task<ServiceResult<T>>> methodInternal)
    {
        var result = await methodInternal();

        if (result.IsError)
        {
            return BadRequest(result.ErrorsMessage);
        }

        return Ok(result);
    }
}
