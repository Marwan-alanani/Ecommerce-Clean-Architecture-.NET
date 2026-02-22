using Microsoft.AspNetCore.Mvc;
using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(Error error)
    {
        HttpContext.Items["error"] = error;
        HttpContext.Items["code"] = error.Code;

        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            title: error.Message,
            statusCode: statusCode,
            detail: error.Description
        );
    }
}