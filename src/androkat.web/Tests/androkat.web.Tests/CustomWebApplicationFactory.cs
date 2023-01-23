using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace androkat.web.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
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

            string inMemoryCollectionName = Guid.NewGuid().ToString();
            services.AddDbContext<AndrokatContext>(options =>
            {
                options.UseInMemoryDatabase(inMemoryCollectionName);
            });
        });
    }

    public static void PopulateTestData(AndrokatContext dbContext)
    {
		foreach (var item in dbContext.Content)
		{
			dbContext.Remove(item);
		}

		foreach (var item in dbContext.RadioMusor)
		{
			dbContext.Remove(item);
		}

		dbContext.Content.Add(new Content { Tipus = 1, Cim = "cím1" });
		dbContext.SaveChanges();

        dbContext.TempContent.Add(new TempContent { Tipus = 1, Cim = "cím1" });
        dbContext.SaveChanges();

        dbContext.RadioMusor.Add(new RadioMusor { Source = "Source1", Musor = "Műsor1" });
		dbContext.SaveChanges();
	}
}