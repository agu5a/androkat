using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class WarmupServiceTests : BaseTest
{
    private Mock<ICacheService> _cacheService = new Mock<ICacheService>();
    private Mock<ILogger<WarmupService>> logger = new Mock<ILogger<WarmupService>>();

    [Test]
    public void MainCacheFillUp_Happy()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.MainCacheFillUp()).Returns(new MainCache
        {
            ContentDetailsModels = new List<ContentDetailsModel> { new ContentDetailsModel { Cim = "cim" } },
            Inserted = DateTime.Now
        });
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        service.MainCacheFillUp();
        var obj = (MainCache)cache.Get(CacheKey.MainCacheKey.ToString());
        obj.ContentDetailsModels.First().Cim.Should().Be("cim");
    }

    [Test]
    public void MainCacheFillUp_Exception()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.MainCacheFillUp()).Throws<Exception>();
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        Action act = () => service.MainCacheFillUp();
        act.Should().NotThrow<Exception>();
    }

    [Test]
    public void BookRadioSysCache_Happy()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.BookRadioSysCacheFillUp()).Returns(new BookRadioSysCache
        {
            Books = new List<ContentDetailsModel> { new ContentDetailsModel { Cim = "cim" } },
            Inserted = DateTime.Now
        });
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        service.BookRadioSysCache();
        var obj = (BookRadioSysCache)cache.Get(CacheKey.BookRadioSysCacheKey.ToString());
        obj.Books.First().Cim.Should().Be("cim");
    }

    [Test]
    public void BookRadioSysCacheFillUp_Exception()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.BookRadioSysCacheFillUp()).Throws<Exception>();
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        Action act = () => service.BookRadioSysCache();
        act.Should().NotThrow<Exception>();
    }

    [Test]
    public void EgyebCacheFillUp_Happy()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.EgyebCacheFillUp()).Returns(new EgyebCache
        {
            Egyeb = new List<ContentDetailsModel> { new ContentDetailsModel { Cim = "cim" } },
            Inserted = DateTime.Now
        });
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        service.EgyebCacheFillUp();
        var obj = (EgyebCache)cache.Get(CacheKey.EgyebCacheKey.ToString());
        obj.Egyeb.First().Cim.Should().Be("cim");
    }

    [Test]
    public void EgyebCacheFillUp_Exception()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.EgyebCacheFillUp()).Throws<Exception>();
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        Action act = () => service.EgyebCacheFillUp();
        act.Should().NotThrow<Exception>();
    }

    [Test]
    public void ImaCacheFillUp_Happy()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.ImaCacheFillUp()).Returns(new ImaCache
        {
            Imak = new List<ImaModel>
            {
                new ImaModel (Guid.NewGuid(), DateTime.MinValue, "cim", "", "")
            },
            Inserted = DateTime.Now
        });
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        service.ImaCacheFillUp();
        var obj = (ImaCache)cache.Get(CacheKey.ImaCacheKey.ToString());
        obj.Imak.First().Cim.Should().Be("cim");
    }

    [Test]
    public void ImaCacheFillUp_Exception()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.ImaCacheFillUp()).Throws<Exception>();
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        Action act = () => service.ImaCacheFillUp();
        act.Should().NotThrow<Exception>();
    }

    [Test]
    public void VideoCacheFillUp_Happy()
    {
        var cache = GetIMemoryCache();
        _cacheService.Setup(s => s.VideoCacheFillUp()).Returns(new VideoCache
        {
            Video = new List<VideoModel>
            {
                new VideoModel(Guid.NewGuid(), "img", "vlink", "cim", DateTime.Now.ToString("yyyy-MM-dd"), "forras", "cId", DateTime.Now)
            },
            Inserted = DateTime.Now
        });
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        service.VideoCacheFillUp();
        var obj = (VideoCache)cache.Get(CacheKey.VideoCacheKey.ToString());
        obj.Video.First().Cim.Should().Be("cim");
    }

    [Test]
    public void VideoCacheFillUp_Exception()
    {
        var cache = GetIMemoryCache();

        _cacheService.Setup(s => s.VideoCacheFillUp()).Throws<Exception>();
        var service = new WarmupService(cache, _cacheService.Object, logger.Object);

        Action act = () => service.VideoCacheFillUp();
        act.Should().NotThrow<Exception>();
    }
}