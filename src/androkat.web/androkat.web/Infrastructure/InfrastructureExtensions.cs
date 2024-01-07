using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.web.Service;
using androkat.web.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace androkat.web.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static void SetDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AndrokatContext>(options =>
        {
            options.UseSqlite("Data Source=./Data/androkat.db");
        });
    }

    public static void SetServices(this IServiceCollection services)
    {
        services.AddScoped<IClock, Clock>();
        services.AddScoped<ICacheRepository, CacheRepository>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IApiRepository, ApiRepository>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddHostedService<Warmup>();
    }

    public static void SetAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }

    public static void SetSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60);
            options.Cookie.Name = "androkat.session";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    public static void UseProxy(this WebApplication app)
    {
        var forwardedHeadersOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        app.UseForwardedHeaders(forwardedHeadersOptions);
    }

    public static void SetCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
    }

    public static void SetAuthentication(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configurationManager["GoogleClientId"]!;
            googleOptions.ClientSecret = configurationManager["GoogleClientSecret"]!;
        });
    }

    public static void SetHealthCheckEndpoint(this WebApplication app)
    {
        var endPoints = app.Services.GetRequiredService<IOptions<EndPointConfiguration>>();
        app.UseHealthChecks(endPoints.Value.HealthCheckApiUrl, new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(s => new HealthCheck
                    {
                        Component = s.Key,
                        Description = s.Value.Description?.ToString(),
                        Status = s.Value.Status.ToString()
                    }),
                    Duration = report.TotalDuration
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        });
    }
}