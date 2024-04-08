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

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Host.UseSerilog();

ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

SetupLogger(app.Environment);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
//});

//app.UseCors("Default");
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


static void SetupLogger(IWebHostEnvironment env)
{
    Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Logger(x =>
                        x.WriteTo.File(
                            Path.Combine(env.ContentRootPath, "logs", "nc_ids.txt"),
                            rollingInterval: RollingInterval.Day,
                            shared: true,
                            flushToDiskInterval: TimeSpan.FromSeconds(5)
                        )
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                    )
                    .CreateLogger();
}

static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration, IWebHostEnvironment env)
{
    var connString = configuration.GetConnectionString("DefaultConnection");

    // Add services to the container.
    services
        .AddControllersWithViews()
        .AddRazorRuntimeCompilation();

    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connString);
        options.UseOpenIddict();
    });

    services.AddIdentity<AppUser, AppRole>(x =>
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

    services.Configure<IdentityOptions>(options =>
    {
        options.ClaimsIdentity.UserNameClaimType = Claims.Name;
        options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
        options.ClaimsIdentity.RoleClaimType = Claims.Role;
    });

    services.AddOpenIddict()

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

            byte[] certificateBytes = File.ReadAllBytes(Path.Combine(env.ContentRootPath, "Certs", "newcenturyids.pfx"));
            X509Certificate2 certificate = new X509Certificate2(certificateBytes, "WFR@indonesia123");

            options.AddSigningCertificate(certificate);
            options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String("5L6Pf+hgOecJKnbqbpSIsfgobBB58CP0quZk6sV1L3s=")));

            // Change default access lifetime
            options.SetAccessTokenLifetime(TimeSpan.FromMinutes(300));
        });

    services.AddCors(opt =>
    {
        opt.AddPolicy("Default", policy =>
        {
            policy.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!);
            policy.AllowAnyHeader().AllowAnyMethod();
        });
    });

    services.AddHttpContextAccessor();
    services.AddScoped<ICurrentUserResolver, CurrentUserResolver>();
}