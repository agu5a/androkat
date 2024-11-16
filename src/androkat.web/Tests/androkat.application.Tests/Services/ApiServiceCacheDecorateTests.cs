using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace androkat.application.Tests.Services;

public class ApiServiceCacheDecorateTests : BaseTest
{
    private readonly Mock<IApiService> _apiServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private ApiServiceCacheDecorate _apiServiceCacheDecorate;

    public ApiServiceCacheDecorateTests()
    {
        _apiServiceMock = new Mock<IApiService>();
        _cacheServiceMock = new Mock<ICacheService>();
    }

    [Fact]
    public void GetEgyebOlvasnivaloByForrasAndNid_ReturnsResultFromCache_WhenResultIsNotNull()
    {
        // Arrange
        var tipus = (int)Forras.kurir;
        var id = Guid.NewGuid();
        var bookRadioSysCache = new BookRadioSysCache();
        var mainCache = new MainCache();
        var expectedContentResponse = new List<ContentResponse> { new() };

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.EgyebOlvasnivaloResponseCacheKey + "_" + tipus + "_" + id, expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetContentByTipusAndId(tipus, id, bookRadioSysCache, mainCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetEgyebOlvasnivaloByForrasAndNid_ReturnsResultFromCache_WhenCacheEmpty()
    {
        // Arrange
        var tipus = (int)Forras.kurir;
        var id = Guid.NewGuid();
        var bookRadioSysCache = new BookRadioSysCache();
        var mainCache = new MainCache();
        var expectedContentResponse = new List<ContentResponse> { new() };

        var cache = GetIMemoryCache();

        _cacheServiceMock.Setup(s => s.MainCacheFillUp()).Returns(mainCache);
        _cacheServiceMock.Setup(s => s.BookRadioSysCacheFillUp()).Returns(bookRadioSysCache);
        _apiServiceMock.Setup(s => s.GetEgyebOlvasnivaloByForrasAndNid(tipus, id, bookRadioSysCache, mainCache)).Returns(expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetEgyebOlvasnivaloByForrasAndNid(tipus, id, bookRadioSysCache, mainCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetContentByTipusAndId_ReturnsResultFromCache_WhenResultIsNotNull()
    {
        // Arrange
        var tipus = (int)Forras.pio;
        var id = Guid.NewGuid();
        var bookRadioSysCache = new BookRadioSysCache();
        var mainCache = new MainCache();
        var expectedContentResponse = new List<ContentResponse> { new() };

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.ContentResponseCacheKey + "_" + tipus + "_" + id, expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetContentByTipusAndId(tipus, id, bookRadioSysCache, mainCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetContentByTipusAndId_ReturnsResultFromCache_WhenCacheEmpty()
    {
        // Arrange
        var tipus = (int)Forras.pio;
        var id = Guid.NewGuid();
        var bookRadioSysCache = new BookRadioSysCache();
        var mainCache = new MainCache();
        var expectedContentResponse = new List<ContentResponse>
        {
            new()
            {
                Nid = id,
                Img = "img",
                Datum = "2022-01-01 01:01:01",
                Forras = "forras"
            }
        };

        var cache = GetIMemoryCache();

        _cacheServiceMock.Setup(s => s.MainCacheFillUp()).Returns(mainCache);
        _apiServiceMock.Setup(s => s.GetContentByTipusAndNid(tipus, id, mainCache)).Returns(expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetContentByTipusAndId(tipus, id, bookRadioSysCache, mainCache);

        // Assert
        Assert.NotNull(result);
        result.First().Nid.Should().Be(id);
        result.First().Img.Should().Be("img");
        result.First().Datum.Should().Be("2022-01-01 01:01:01");
        result.First().Forras.Should().Be("forras");
    }

    [Fact]
    public void GetRadioBySource_ReturnsResultFromCache_WhenCacheEmpty()
    {
        // Arrange
        var radio = "katolikusradio";
        var id = Guid.NewGuid();
        var bookRadioSysCache = new BookRadioSysCache();
        var mainCache = new MainCache();
        var expectedContentResponse = new List<RadioMusorResponse> { new() };

        var cache = GetIMemoryCache();

        _cacheServiceMock.Setup(s => s.BookRadioSysCacheFillUp()).Returns(bookRadioSysCache);
        _apiServiceMock.Setup(s => s.GetRadioBySource(radio, bookRadioSysCache)).Returns(expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetRadioBySource(radio, bookRadioSysCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetImaByDate_ReturnsResultFromCache_WhenResultIsNotNull()
    {
        // Arrange
        var date = "2022-01-01 01:01:01";
        var imaCache = new ImaCache();
        var expectedImaResponse = new ImaResponse
        {
            HasMore = false,
            Imak = new List<ImaDetailsResponse> { new() }
        };

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.ImaResponseCacheKey + "_2022-01-01_01:01:01 ", expectedImaResponse);

        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetImaByDate(date, imaCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetImaByDate_ReturnsResultFromCache_WhenCacheEmpty()
    {
        // Arrange
        var date = "2022-01-01 01:01:01";
        var imaCache = new ImaCache() { Imak = new List<ImaModel>(), Inserted = DateTime.Now };
        var nid = Guid.NewGuid();
        var expectedImaResponse = new ImaResponse
        {
            HasMore = false,
            Imak = new List<ImaDetailsResponse>
            {
                new()
                {
                    Nid = nid,
                    Csoport = 1,
                    Leiras = "leiras",
                    Time = "2022-01-01 01:01:01"
                }
            }
        };

        var cache = GetIMemoryCache();

        _cacheServiceMock.Setup(s => s.ImaCacheFillUp()).Returns(imaCache);
        _apiServiceMock.Setup(s => s.GetImaByDate(date, It.IsAny<ImaCache>())).Returns(expectedImaResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetImaByDate(date, imaCache);

        // Assert
        Assert.NotNull(result);
        result.HasMore.Should().BeFalse();
        result.Imak.First().Nid.Should().Be(nid);
        result.Imak.First().Csoport.Should().Be(1);
        result.Imak.First().Leiras.Should().Be("leiras");
        result.Imak.First().Time.Should().Be("2022-01-01 01:01:01");
    }

    [Fact]
    public void GetSystemData_ReturnsResultFromCache_WhenResultIsNotNull()
    {
        // Arrange
        var bookRadioSysCache = new BookRadioSysCache();
        var expectedContentResponse = new List<SystemDataResponse> { new() };
        bookRadioSysCache.SystemData = new List<SystemInfoModel> { new(1, "", "") };

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.BookRadioSysCacheKey.ToString(), bookRadioSysCache);
        _apiServiceMock.Setup(s => s.GetSystemData(bookRadioSysCache)).Returns(expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetSystemData(bookRadioSysCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetSystemData_ReturnsResultFromCache_WhenCacheEmpty()
    {
        // Arrange
        var bookRadioSysCache = new BookRadioSysCache();
        var mainCache = new MainCache();
        var expectedContentResponse = new List<SystemDataResponse> { new() };

        var cache = GetIMemoryCache();

        _cacheServiceMock.Setup(s => s.BookRadioSysCacheFillUp()).Returns(bookRadioSysCache);
        _apiServiceMock.Setup(s => s.GetSystemData(bookRadioSysCache)).Returns(expectedContentResponse);
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetSystemData(bookRadioSysCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetVideoForWebPage_ReturnsResultFromCache_WhenResultIsNotNull()
    {
        // Arrange
        var f = "channel";
        var offset = 0;
        var videoCache = new VideoCache();

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.VideoResponseCacheKey + "_" + f + "_" + offset, "video");
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetVideoForWebPage(f, offset, videoCache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetVideoForWebPage_ReturnsResultFromCache_WhenCacheEmpty()
    {
        // Arrange
        var f = "channel";
        var offset = 0;
        var videoCache = new VideoCache();

        var cache = GetIMemoryCache();

        _cacheServiceMock.Setup(s => s.VideoCacheFillUp()).Returns(videoCache);
        _apiServiceMock.Setup(s => s.GetVideoForWebPage(f, offset, videoCache)).Returns("video");
        _apiServiceCacheDecorate = new ApiServiceCacheDecorate(_apiServiceMock.Object, cache, _cacheServiceMock.Object);

        // Act
        var result = _apiServiceCacheDecorate.GetVideoForWebPage(f, offset, videoCache);

        // Assert
        Assert.NotNull(result);
    }
}
