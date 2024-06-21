using LearnFrameworkApi.WorkerService;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using LearnFrameworkApi.Module.Datas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LearnFrameworkApi.Module.Services.Configuration;
using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = Host.CreateDefaultBuilder(args).UseWindowsService();

// Setup Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs", "learnApiFramework-.txt"),
        rollingInterval: RollingInterval.Day,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(5)
    )
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs", "learnApiFrameworkError-.txt"),
        rollingInterval: RollingInterval.Day,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(5),
        restrictedToMinimumLevel: LogEventLevel.Error
    )
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();
builder.UseSerilog();

// Configure Service
builder.ConfigureServices((hostContext, services) =>
{
    var configuration = hostContext.Configuration;
    services.AddHostedService<Worker>();
    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    services.AddScoped<ICurrentUserResolver, CurrentUserResolver>();
});

// Build and Run
var host = builder.Build();
try
{
    Log.Information("Starting up the service");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service");
}
finally
{
    Log.CloseAndFlush();
}