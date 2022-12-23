using androkat.application.DI;
using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Configuration;
using androkat.infrastructure.Configuration;
using androkat.web.Infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Text.Json.Serialization;

#pragma warning disable S4792
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
#pragma warning restore S4792

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Host.UseSerilog((ctx, lc) =>
    {
        lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .Enrich.WithProperty("MyApp", "AndroKatWeb");
    });

    builder.Services.SetAutoMapper();
    builder.Services.SetSession();

    builder.Services.AddSingleton<IConfigureOptions<AndrokatConfiguration>, AndrokatConfigurationOptions>();
    builder.Services.AddSingleton<IContentMetaDataService, ContentMetaDataService>();

    builder.Services.AddControllers()
        .AddRazorRuntimeCompilation().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new DefaultCoreModule());
    });

    builder.Services.SetServices();
    builder.Services.AddRazorPages();

    var app = builder.Build();

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
    app.UseHttpsRedirection();

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
    app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}");
    app.MapRazorPages();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Stopped program because of exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}