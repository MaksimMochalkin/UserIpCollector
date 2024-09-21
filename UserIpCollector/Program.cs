using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using UserIpCollector.Abstractions.Interfaces;
using UserIpCollector.Data;
using UserIpCollector.Helpers;
using UserIpCollector.Middleware;
using UserIpCollector.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(LoggerHelper.GetLogFilePath(), rollingInterval: RollingInterval.Day)
    .CreateLogger();

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));

builder.Services.AddDbContext<ApplicationDbContext>(config =>
{
    config.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
});

builder.Services.AddDistributedMemoryCache();
//builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserIpAdresesService, UserIpAdresesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
