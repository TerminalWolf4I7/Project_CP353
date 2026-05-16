using Delivery.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register EF Core with PostgreSQL
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=DeliveryDB"));

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Delivery API running");

app.Run();
