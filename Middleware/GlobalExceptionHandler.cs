using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public GlobalExceptionHandler()
    {
        
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

        var problemDetails = new ProblemDetails()
        {
            Type = exception.GetType().Name,
        };


        switch (exception)
        {
            case BadHttpRequestException:
                problemDetails.Detail = "Bad Request";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            case UnauthorizedAccessException:
                problemDetails.Detail = "You are not authoried.";
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                break;
            default:
                problemDetails.Detail = "Internal Server Error";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return false;
    }
}
