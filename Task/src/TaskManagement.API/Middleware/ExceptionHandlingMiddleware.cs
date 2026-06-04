using System.Net;
using System.Text.Json;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Exceptions;

namespace TaskManagement.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException ex)
        {
            logger.LogWarning(ex, "Handled application exception");
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            var response = ApiResponse<object>.Fail(ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = ApiResponse<object>.Fail("An unexpected error occurred.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

