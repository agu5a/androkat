using androkat.infrastructure.DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace androkat.infrastructure.Tests;

public class BaseTest
{
    public IMemoryCache GetIMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        return memoryCache;
    }

    public DbContextOptions<AndrokatContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<AndrokatContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }
}
