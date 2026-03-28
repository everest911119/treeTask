using Backend.Data;
using Backend.Exceptions; // Add this if not already present
using Backend.Filter;
using Backend.Repo;
using HealthChecks.UI.Client;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Events;
using Serilog.Sinks;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddScoped<TreeTaskRepo>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("c:/temp/treeTask.log")
        .CreateLogger();
    loggingBuilder.AddSerilog();
});
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CacheActionFilter>();
builder.Services.AddCors(c =>
{
    string[] urls = ["http://127.0.0.1:3000", "http://localhost:3000/", "http://localhost", "http://localhost:3000"];
    c.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                            .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});
builder.Services.AddHealthChecks().AddSqlServer(connectionString, name: "Database", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseExceptionHandler(opt => { });
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
//app.UseMiddleware<Backend.Middleware.GlobalExceptionMiddleware>(); easy approach
app.Run();
