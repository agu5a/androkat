using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace androkat.application.Tests.Services;

public class ContentServiceTests : BaseTest
{
    private readonly Mock<ICacheService> _cacheServiceMock;

    public ContentServiceTests()
    {
        _cacheServiceMock = new Mock<ICacheService>();
    }

    [Fact]
    public void GetAudio()
    {
        // Arrange
        var mainCache = new MainCache()
        {
            ContentDetailsModels = new List<ContentDetailsModel>
            {
                new(Guid.NewGuid(), DateTime.Now, "cim", "idezet", (int)Forras.prayasyougo, DateTime.Now),
                new(Guid.NewGuid(), DateTime.Now, "cim", "idezet", (int)Forras.audiohorvath, DateTime.Now)
            }
        };

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.MainCacheKey.ToString(), mainCache);

        var service = new ContentService(cache, _cacheServiceMock.Object, GetAndrokatConfiguration());

        // Act
        var result = service.GetAudio();

        // Assert
        Assert.Equal(2, result.Count);
    }
}
