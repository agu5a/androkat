using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Configuration;
using androkat.infrastructure.Configuration;
using androkat.infrastructure.DataManager;
using androkat.web.Attributes;
using androkat.web.Service;
using FastEndpoints;
using FastEndpoints.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace androkat.web.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor().AddScoped<IAuthService, AuthService>();

        builder.Services.SetAutoMapper();
        builder.Services.SetCaching();
        builder.Services.SetSession();
        builder.Services.SetDatabase();

        builder.Services.AddSingleton<IConfigureOptions<AndrokatConfiguration>, AndrokatConfigurationOptions>();
        builder.Services.AddOptions<EndPointConfiguration>().BindConfiguration("EndPointConfiguration").ValidateDataAnnotations().ValidateOnStart();
        builder.Services.AddOptions<CredentialConfiguration>().BindConfiguration("CredentialConfiguration").ValidateDataAnnotations().ValidateOnStart();
        builder.Services.AddOptions<GeneralConfiguration>().BindConfiguration("GeneralConfiguration").ValidateDataAnnotations().ValidateOnStart();
        builder.Services.AddSingleton<IContentMetaDataService, ContentMetaDataService>();
        builder.Services.SetAuthentication(builder.Configuration);
        builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
        builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", o =>
            {
                o.WithOrigins("https://androkat.hu").AllowAnyHeader().AllowAnyMethod();
            });
        });

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy("fixed-by-ip", httpContext => 
                RateLimitPartition.GetFixedWindowLimiter
                (
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 40,
                        Window = TimeSpan.FromMinutes(1)
                    }
                    ));
        });

        builder.Services.AddControllers()
            .AddRazorRuntimeCompilation().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        builder.Services.SetServices();
        builder.Services.AddRazorPages();
        builder.Services.AddFastEndpoints();
        builder.Services.AddFastEndpointsApiExplorer();

        builder.Services.AddHealthChecks().AddDbContextCheck<AndrokatContext>();
    }
}