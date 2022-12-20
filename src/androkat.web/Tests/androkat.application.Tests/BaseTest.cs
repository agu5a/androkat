using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System;

namespace androkat.application.Tests;

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

    public static Mock<IContentRepository> GetContentRepository(List<ContentModel> list)
    {
        Mock<IContentRepository> repository = new();
        repository.Setup(s => s.GetContentDetailsModel(It.IsAny<int[]>())).Returns(list);

        return repository;
    }
}
