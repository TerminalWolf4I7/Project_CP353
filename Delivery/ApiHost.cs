using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Delivery
{
    internal static class ApiHost
    {
        public static WebApplication Start()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();

            builder.WebHost.UseUrls("http://localhost:5000");

            var app = builder.Build();
            app.MapControllers();
            app.MapGet("/", () => "Delivery API running");

            app.Start();
            return app;
        }
    }
}
