using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;

namespace androkat.application.Tests;

public class BaseTest
{
    public DbContextOptions<AndrokatContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<AndrokatContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }

    public IMemoryCache GetIMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        return memoryCache;
    }

    public static Mock<IContentRepository> GetContentRepository(List<ContentModel> list)
    {
        Mock<IContentRepository> repository = new();
        repository.Setup(s => s.GetContentDetailsModel(It.IsAny<int[]>())).Returns(list);

        return repository;
    }
}