using androkat.businesslayer.Mapper;
using androkat.businesslayer.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace androkat.core.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static IServiceCollection SetServices(this IServiceCollection services)
    {
        services.AddScoped<IMainPageService, MainPageService>();

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
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }
}