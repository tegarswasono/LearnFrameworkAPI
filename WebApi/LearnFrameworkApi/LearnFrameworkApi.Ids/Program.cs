using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Cryptography.X509Certificates;
using LearnFrameworkApi.Module.Helpers;
using LearnFrameworkApi.Ids;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
SetupService(builder);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

RunMigration(app.Services);

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
                Path.Combine(builder.Environment.ContentRootPath, "logs", "nc_ids.txt"),
                rollingInterval: RollingInterval.Day,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(5)
            )
            .WriteTo.File(
                Path.Combine(builder.Environment.ContentRootPath, "logs", "learnApiFrameworkError-.txt"),
                rollingInterval: RollingInterval.Day,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(5),
                restrictedToMinimumLevel: LogEventLevel.Error
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
    builder.Services.AddIdentity<AppUser, AppRole>(x =>
    {
        x.Password.RequireUppercase = false;
        x.Password.RequireDigit = false;
        x.Password.RequiredLength = 5;
        x.Password.RequireNonAlphanumeric = false;
        x.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
    .AddDefaultTokenProviders();

    //OpenIdDict
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.ClaimsIdentity.UserNameClaimType = Claims.Name;
        options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
        options.ClaimsIdentity.RoleClaimType = Claims.Role;
    });
    builder.Services.AddOpenIddict()

        // Register the OpenIddict core components.
        .AddCore(options =>
        {
            options.UseEntityFrameworkCore()
                   .UseDbContext<AppDbContext>();
        })
        // Register the OpenIddict server components.
        .AddServer(options =>
        {
            // Enable the authorization, logout, token and userinfo endpoints.
            options.SetAuthorizationEndpointUris("/connect/authorize")
                       .SetTokenEndpointUris("/connect/token")
                       .SetIntrospectionEndpointUris("/connect/introspect");

            // Mark the "email", "profile" and "roles" scopes as supported scopes.
            options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

            options.AllowAuthorizationCodeFlow()
                .AllowHybridFlow()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .AcceptAnonymousClients();

            // Register the signing and encryption credentials.
            //options.DisableAccessTokenEncryption();
            //options.RequireProofKeyForCodeExchange();

            // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
            options.UseAspNetCore()
                   .EnableAuthorizationEndpointPassthrough()
                   .EnableTokenEndpointPassthrough()
                   .EnableStatusCodePagesIntegration()
                   .DisableTransportSecurityRequirement();

            byte[] certificateBytes = File.ReadAllBytes(Path.Combine(builder.Environment.ContentRootPath, "Certs", "newcenturyids.pfx"));
            X509Certificate2 certificate = new X509Certificate2(certificateBytes, "WFR@indonesia123");

            options.AddSigningCertificate(certificate);
            options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String("5L6Pf+hgOecJKnbqbpSIsfgobBB58CP0quZk6sV1L3s=")));

            // Change default access lifetime
            options.SetAccessTokenLifetime(TimeSpan.FromMinutes(300));
        });

    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("Default", policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!);
            policy.AllowAnyHeader().AllowAnyMethod();
        });
    });

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserResolver, CurrentUserResolver>();
}
static void RunMigration(IServiceProvider service)
{
    using (var scope = service.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        context.Database.Migrate();
    }
}