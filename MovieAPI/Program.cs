using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using Npgsql;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5001");

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<MovieRepository>();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Apply migrations automatically with retry logic
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
    var retryCount = 5;
    while (retryCount > 0)
    {
        try
        {
            dbContext.Database.Migrate(); // Apply pending migrations
            Console.WriteLine("✅ Database connected successfully!");
            break;
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine($"⚠️ Database connection failed. Retrying... ({retryCount} attempts left)");
            retryCount--;
            System.Threading.Thread.Sleep(5000); // Wait 5 seconds before retrying
        }
    }

    if (retryCount == 0)
    {
        Console.WriteLine("❌ Failed to connect to database after multiple attempts.");
    }
}

app.Run();
