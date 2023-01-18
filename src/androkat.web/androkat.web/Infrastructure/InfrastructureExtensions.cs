using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.web.Core;
using androkat.web.Service;
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
    public static IServiceCollection SetDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AndrokatContext>(options =>
        {
            options.UseSqlite($"Data Source=./Data/androkat.db");
        }, ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection SetServices(this IServiceCollection services)
    {
        services.AddScoped<IClock, Clock>();
        services.AddScoped<ICacheRepository, CacheRepository>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IApiRepository, ApiRepository>();
        services.AddHostedService<Warmup>();

        return services;
    }

    public static IServiceCollection SetAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));

        return services;
    }

    public static IServiceCollection SetSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60);
            options.Cookie.Name = "androkat.session";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }

    public static WebApplication UseProxy(this WebApplication app)
    {
        var forwardedHeadersOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        };
        app.UseForwardedHeaders(forwardedHeadersOptions);

        return app;
    }

    public static IServiceCollection SetCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        return services;
    }

    public static IServiceCollection SetAuthentication(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configurationManager["GoogleClientId"]; ;
            googleOptions.ClientSecret = configurationManager["GoogleClientSecret"]; ;
        });

        return services;
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