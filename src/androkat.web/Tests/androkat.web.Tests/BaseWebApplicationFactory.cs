using androkat.application.Interfaces;
using androkat.infrastructure.DataManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace androkat.web.Tests;

public abstract class BaseWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        var host = builder.Build();
        host.Start();

        var serviceProvider = host.Services;

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AndrokatContext>();

            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            db.Database.EnsureCreated();

            PopulateTestData(db);

            var cache = scopedServices.GetRequiredService<IWarmupService>();
            cache.BookRadioSysCache();
            cache.MainCacheFillUp();
            cache.ImaCacheFillUp();
            cache.VideoCacheFillUp();
        }

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AndrokatContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            var inMemoryCollectionName = Guid.NewGuid().ToString();
            services.AddDbContext<AndrokatContext>(options =>
            {
                options.UseInMemoryDatabase(inMemoryCollectionName);
            });
        });
    }

    public abstract void PopulateTestData(AndrokatContext dbContext);
}