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

    string logzioToken = Environment.GetEnvironmentVariable("ANDROKAT_CREDENTIAL_LOGZIO_TOKEN") ?? string.Empty;

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

#pragma warning disable S1075 // URIs should not be hardcoded

        loggerConfiguration
        .ReadFrom.Configuration(hostBuilderContext.Configuration, options)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.LogzIoDurableHttp(
            $"https://listener.logz.io:8071/?type=app&token={logzioToken}",
            logzioTextFormatterOptions: null)
        .Enrich.WithProperty("MyApp", "AndroKatWeb");
#pragma warning restore S1075 // URIs should not be hardcoded

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