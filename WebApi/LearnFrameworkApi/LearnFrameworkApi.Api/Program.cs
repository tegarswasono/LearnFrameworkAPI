using LearnFrameworkApi.Api.Helpers;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers;
using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Data;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);
SetupService(builder);

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

app.UseCors("Default");

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();

static void SetupService(WebApplicationBuilder? builder)
{
    //Serilog
    builder!.Host.UseSerilog();
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Logger(x =>
            x.WriteTo.File(
                Path.Combine(builder.Environment.ContentRootPath, "logs", "learnApiFramework-.txt"),
                rollingInterval: RollingInterval.Day,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(5)
            )
            .WriteTo.File(
                Path.Combine(builder.Environment.ContentRootPath, "logs", "learnApiFrameworkError-.txt"),
                rollingInterval: RollingInterval.Day,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(5),
                restrictedToMinimumLevel : LogEventLevel.Error
            )
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
        )
        .CreateLogger();

    //DbContext
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connString);
        options.UseOpenIddict();
    });

    //Identity
    builder.Services.AddIdentity<AppUser, AppRole>(x =>
    {
        x.Password.RequireUppercase = false;
        x.Password.RequireDigit = false;
        x.Password.RequiredLength = 5;
        x.Password.RequireNonAlphanumeric = false;
        x.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    //OpenIddict
    builder.Services.AddOpenIddict()
        // Register the OpenIddict server components.
        .AddValidation(options =>
        {
            options.UseAspNetCore();
            options.UseSystemNetHttp();
            options.SetIssuer(builder.Configuration.GetSection("AuthIssuer").Value!);

            byte[] certificateBytes = System.IO.File.ReadAllBytes(Path.Combine(builder.Environment.ContentRootPath, "Certs", "newcenturyids.pfx"));
            X509Certificate2 certificate = new X509Certificate2(certificateBytes, "WFR@indonesia123");
            options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String("5L6Pf+hgOecJKnbqbpSIsfgobBB58CP0quZk6sV1L3s=")));
            //options.AddEventHandler(CustomProcessRequestContext.Descriptor);
        });

    //Cors
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("Default", policy =>
        {
            policy.AllowCredentials();
            policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!);
            policy.AllowAnyHeader().AllowAnyMethod();
        });
    });

    //SmtpClient
    builder.Services.AddTransient<SmtpClient>(sp =>
    {
        var context = sp.GetRequiredService<AppDbContext>();
        var smtpSetting = context.SmtpSettings.FirstOrDefault();
        if (smtpSetting != null)
        {
            var smtpClient = new SmtpClient(smtpSetting.SmtpServer, smtpSetting.SmtpPort)
            {
                Credentials = new NetworkCredential(smtpSetting.SmtpUser, smtpSetting.SmtpPassword),
                EnableSsl = smtpSetting.SmtpIsUseSsl
            };
            return smtpClient;
        }
        else
        {
            return new SmtpClient();
        }
    });
    builder.Services.AddTransient<EmailService>();

    //Memory Cache
    builder.Services.AddMemoryCache();

    //UserManager Reset Token
    builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromHours(1);
    });


    //service
    builder.Services.AddScoped<ICurrentUserResolver, CurrentUserResolver>();
}