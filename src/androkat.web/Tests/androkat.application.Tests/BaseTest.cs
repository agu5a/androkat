using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Configuration;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.infrastructure.DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;

namespace androkat.application.Tests;

public class BaseTest
{
	public static IOptions<AndrokatConfiguration> GetAndrokatConfiguration()
	{
		var logger = new Mock<ILogger<ContentMetaDataService>>();
		var service = new ContentMetaDataService(logger.Object);
		var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

		var contentMetaDataModels = Options.Create(new AndrokatConfiguration
		{
			ContentMetaDataList = metaDataList
		});

		return contentMetaDataModels;
	}

	public static Mock<IClock> GetClock()
	{
		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));
		return clock;
	}

	public static DbContextOptions<AndrokatContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<AndrokatContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }

	public static IMemoryCache GetIMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        return memoryCache;
    }

    public static Mock<ICacheService> GetCacheService(List<ContentModel> list)
    {
        Mock<ICacheService> repository = new();
        repository.Setup(s => s.MainCacheFillUp()).Returns(new MainCache
        {
            ContentDetailsModels = new List<ContentDetailsModel>
            {
                list[0].ContentDetails
            }
        });

        return repository;
    }
}
