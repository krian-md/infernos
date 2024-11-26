using System.Text.Json;
using Infernos.Shared.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infernos.Shared.Middlewares;

public class GlobalException(RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context)
    {
        int statusCode;
        string message;
        string title;

        try
        {
            await next(context);

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status429TooManyRequests:
                    title = "Warning";
                    message = "Too many request made.";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    break;
                case StatusCodes.Status401Unauthorized:
                    title = "Alert";
                    message = "You are not authorized to access.";
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case StatusCodes.Status403Forbidden:
                    title = "Out of Access";
                    message = "You are not allowed/required to access.";
                    statusCode = StatusCodes.Status403Forbidden;
                    break;
                default:
                    title = "Error";
                    message = "Sorry, internal server error occurred. Kindly try again";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            await ModifyHeader(context, title, message, statusCode);
        }
        catch (Exception ex)
        {
            // Log Original Exception / File, Debugger, Console
            Logger.LogExceptions(ex);

            // Check if exception if Timeout
            if (ex is TaskCanceledException || ex is TimeoutException)
            {
                title = "Out of time";
                message = "Sorry, timeout. Kindly try again";
                statusCode = StatusCodes.Status408RequestTimeout;
            }
            else
            {
                title = "Error";
                message = "Sorry, internal server error occurred. Kindly try again";
                statusCode = StatusCodes.Status500InternalServerError;
            }

            await ModifyHeader(context, title, message, statusCode);
        }
    }


    private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
        {
            Detail = message,
            Status = statusCode,
            Title = title,
        }), CancellationToken.None);
        return;
    }
}