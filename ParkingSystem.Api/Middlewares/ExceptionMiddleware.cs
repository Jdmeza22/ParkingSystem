using ParkingSystem.Application.Common;
using ParkingSystem.Application.Exceptions;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace ParkingSystem.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred. TraceIdentifier: {TraceIdentifier}",   context.TraceIdentifier);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync( HttpContext context,Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            BadRequestException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            ForbiddenException => HttpStatusCode.Forbidden,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;
        var response = ApiResponse<object>.ErrorResponse(exception.Message );

        var message = statusCode == HttpStatusCode.InternalServerError ? "An unexpected error occurred." : exception.Message;


        var json = JsonSerializer.Serialize(response,
            new JsonSerializerOptions
                {
                    PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase
                });

        await context.Response.WriteAsync(json);
    }
}