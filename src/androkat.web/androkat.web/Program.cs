using androkat.application.DI;
using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Configuration;
using androkat.infrastructure.Configuration;
using androkat.infrastructure.DataManager;
using androkat.web.Infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Reflection;
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

    builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
    {
        loggerConfiguration
        .ReadFrom.Configuration(hostBuilderContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .Enrich.WithProperty("MyApp", "AndroKatWeb");
    });

    builder.Services.SetAutoMapper();
    builder.Services.SetCaching();
    builder.Services.SetSession();
    builder.Services.SetDatabase();

    builder.Services.AddSingleton<IConfigureOptions<AndrokatConfiguration>, AndrokatConfigurationOptions>();
    builder.Services.AddOptions<EndPointConfiguration>().BindConfiguration("EndPointConfiguration").ValidateDataAnnotations().ValidateOnStart();
    builder.Services.AddOptions<CredentialConfiguration>().BindConfiguration("CredentialConfiguration").ValidateDataAnnotations().ValidateOnStart();
    builder.Services.AddSingleton<IContentMetaDataService, ContentMetaDataService>();
    //builder.Services.SetAuthentication(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.WithOrigins("https://androkat.hu").AllowAnyHeader().AllowAnyMethod();
        });
    });
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
    builder.Services.AddFastEndpoints();
    builder.Services.AddFastEndpointsApiExplorer();

    builder.Services.AddHealthChecks().AddDbContextCheck<AndrokatContext>();

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
    app.UseCors("CorsPolicy");
    app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}");
    app.MapRazorPages();
    app.UseFastEndpoints();

    app.Lifetime.ApplicationStopping.Register(() =>
    {
        Log.Information(Environment.StackTrace);
    });

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