using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class WarmupServiceTests : BaseTest
{
    private readonly Mock<ICacheService> _cacheService = new();
    private readonly Mock<ILogger<WarmupService>> logger = new();

	[Fact]
	public void MainCacheFillUp_Happy()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.MainCacheFillUp()).Returns(new MainCache
		{
			ContentDetailsModels = new List<ContentDetailsModel> 
			{
				new(Guid.Empty, DateTime.MinValue, "cim", string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
			},
			Inserted = DateTime.Now
		});
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		service.MainCacheFillUp();
		var obj = (MainCache)cache.Get(CacheKey.MainCacheKey.ToString());
		obj.ContentDetailsModels.First().Cim.Should().Be("cim");
	}

	[Fact]
	public void MainCacheFillUp_Exception()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.MainCacheFillUp()).Throws<Exception>();
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		Action act = () => service.MainCacheFillUp();
		act.Should().NotThrow<Exception>();
	}

	[Fact]
	public void BookRadioSysCache_Happy()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.BookRadioSysCacheFillUp()).Returns(new BookRadioSysCache
		{
			Books = new List<ContentDetailsModel> 
			{
				new(Guid.Empty, DateTime.MinValue, "cim", string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
            },
			Inserted = DateTime.Now
		});
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		service.BookRadioSysCache();
		var obj = (BookRadioSysCache)cache.Get(CacheKey.BookRadioSysCacheKey.ToString());
		obj.Books.First().Cim.Should().Be("cim");
	}

	[Fact]
	public void BookRadioSysCacheFillUp_Exception()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.BookRadioSysCacheFillUp()).Throws<Exception>();
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		Action act = () => service.BookRadioSysCache();
		act.Should().NotThrow<Exception>();
	}


	[Fact]
	public void ImaCacheFillUp_Happy()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.ImaCacheFillUp()).Returns(new ImaCache
		{
			Imak = new List<ImaModel>
			{
				new(Guid.NewGuid(), DateTime.MinValue, "cim", "", "")
			},
			Inserted = DateTime.Now
		});
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		service.ImaCacheFillUp();
		var obj = (ImaCache)cache.Get(CacheKey.ImaCacheKey.ToString());
		obj.Imak.First().Cim.Should().Be("cim");
	}

	[Fact]
	public void ImaCacheFillUp_Exception()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.ImaCacheFillUp()).Throws<Exception>();
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		Action act = () => service.ImaCacheFillUp();
		act.Should().NotThrow<Exception>();
	}

	[Fact]
	public void VideoCacheFillUp_Happy()
	{
		var cache = GetIMemoryCache();
		_cacheService.Setup(s => s.VideoCacheFillUp()).Returns(new VideoCache
		{
			Video = new List<VideoModel>
			{
				new(Guid.NewGuid(), "img", "vlink", "cim", DateTime.Now.ToString("yyyy-MM-dd"), "forras", "cId", DateTime.Now)
			},
			Inserted = DateTime.Now
		});
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		service.VideoCacheFillUp();
		var obj = (VideoCache)cache.Get(CacheKey.VideoCacheKey.ToString());
		obj.Video.First().Cim.Should().Be("cim");
	}

	[Fact]
	public void VideoCacheFillUp_Exception()
	{
		var cache = GetIMemoryCache();

		_cacheService.Setup(s => s.VideoCacheFillUp()).Throws<Exception>();
		var service = new WarmupService(cache, _cacheService.Object, logger.Object);

		Action act = () => service.VideoCacheFillUp();
		act.Should().NotThrow<Exception>();
	}
}