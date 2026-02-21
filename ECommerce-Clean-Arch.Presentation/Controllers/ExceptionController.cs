using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

public class ExceptionController : ApiController
{

    [Route("/error")]
    public IActionResult Handle()
    {
        var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionDetails?.Error;
        // _logger.LogError(exception, exception.Message);

        return Problem(title: exception?.Message, statusCode: 500);
    }
}