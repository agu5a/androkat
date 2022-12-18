using androkat.domain;
using androkat.infrastructure.DataManager.SQLite;
using androkat.infrastructure.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace androkat.web.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static IServiceCollection SetServices(this IServiceCollection services)
    {
        services.AddScoped<ISQLiteRepository, SQLiteRepository>();
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
}