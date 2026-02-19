using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected ProblemDetails Problem(
        string? detail = null,
        string? instance = null,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        IEnumerable<IError>? errors = null
    )
    {
        Dictionary<string, Object?> errorDict = new();
        if (errors != null)
        {
        }

        return new ProblemDetails()
        {
            Detail = detail,
            Instance = instance,
            Status = statusCode,
            Title = title,
            Type = type,
            Extensions = errorDict
        };
    }
}