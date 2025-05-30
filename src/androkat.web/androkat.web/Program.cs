using androkat.application.DI;
using androkat.web.Infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Expressions;
using Serilog.Settings.Configuration;
using System;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
    });

    builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
    {
        var options = new ConfigurationReaderOptions(
            typeof(ConsoleLoggerConfigurationExtensions).Assembly,
            typeof(SerilogExpression).Assembly
        );
        
        loggerConfiguration
        .ReadFrom.Configuration(hostBuilderContext.Configuration, options)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .Enrich.WithProperty("MyApp", "AndroKatWeb");
    });

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new DefaultCoreModule());
    });

    builder.AddServices();

    var app = builder.ConfigureWebApplication();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Stopped program because of exception");
}
finally
{
    Log.Information("Shut down complete");
    await Log.CloseAndFlushAsync();
}