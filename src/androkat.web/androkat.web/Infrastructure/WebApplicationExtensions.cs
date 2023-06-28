using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace androkat.web.Infrastructure;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureWebApplication(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        Log.Information("androkat.web version: " + Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);

        if (app.Environment.IsDevelopment())
        {
#pragma warning disable S4507
            app.UseDeveloperExceptionPage();
#pragma warning restore S4507
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
        }

        app.UseProxy();
        app.SetHealthCheckEndpoint();

        app.UseSecurityHeaders(); //adding security headers

        var provider = new FileExtensionContentTypeProvider();
        provider.Mappings[".epub"] = "application/epub+zip";
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = provider
        });

        app.UseRouting();
        app.UseSession(); //Call UseSession after UseRouting and before MapRazorPages

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("CorsPolicy");
        app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}");
        app.MapRazorPages();
        app.UseFastEndpoints();

        app.Lifetime.ApplicationStopping.Register(() =>
        {
            Log.Information(Environment.StackTrace);
        });

        return app;
    }
}
