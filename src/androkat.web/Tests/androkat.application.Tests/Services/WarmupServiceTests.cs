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
}