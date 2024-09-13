using System.Text;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using uatp;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


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
            if (authHeader.StartsWith("Basic "))
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Substring(6))).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                if (username == "admin" && password == "password") 
                {
                    await _next(context);
                    return;
                }
            }
        }
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
    public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
    {
        app.UseMiddleware<BasicAuthMiddleware>();
    }
}