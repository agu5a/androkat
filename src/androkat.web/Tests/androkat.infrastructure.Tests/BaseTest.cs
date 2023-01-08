using androkat.application.Interfaces;
using androkat.infrastructure.DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace androkat.infrastructure.Tests;

public class BaseTest
{
	/// <summary>
	/// DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"
	/// </summary>
	/// <returns></returns>
	public static Mock<IClock> GetToday()
	{
		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));
		return clock;
	}

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
