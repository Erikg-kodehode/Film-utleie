using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Add database connection
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlite("Data Source=movies.db"));

builder.Services.AddScoped<MovieRepository>(); // Register Repository

builder.Services.AddControllers();
var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Run DB Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
    dbContext.Database.EnsureCreated(); // Creates DB if not exists
}

app.Run();
