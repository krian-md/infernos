using Microsoft.AspNetCore.Http;

namespace Infernos.Shared.Middlewares;

public class ApiGateway(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Extract specific headers from the request
        var headers = context.Request.Headers["Api-Gateway"];

        // NULL means, the request is not coming from the Api Gateway
        if (headers.FirstOrDefault() is null)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Sorry, service is unavailable");
            return;
        }
        else
        {
            await next(context);
        }
    }
}