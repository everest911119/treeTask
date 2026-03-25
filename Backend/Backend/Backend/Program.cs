using Backend.Data;
using Backend.Repo;
using Microsoft.Data.SqlClient;
using System.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks;
using Microsoft.Extensions.Caching.Memory;
using Backend.Filter; // Add this if not already present

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddScoped<TreeTaskRepo>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("c:/temp/treeTask.log")
        .CreateLogger();
});
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CacheActionFilter>();
builder.Services.AddCors(c =>
{
    string[] urls = ["http://127.0.0.1:3000", "http://localhost:3000/", "http://localhost", "http://localhost:3000"];
    c.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                            .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();
