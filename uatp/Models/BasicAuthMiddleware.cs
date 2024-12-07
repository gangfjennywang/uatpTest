using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Substring(6))).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];

                    // Replace with your authentication logic
                    if (username == "admin" && password == "password")
                    {
                        await _next(context);
                        return;
                    }
                }
                catch
                {
                    // Handle any exceptions related to base64 conversion or splitting
                }
            }
        }
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"myrealm\"";
    }
}
