using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace androkat.application.Tests.Services;

public class ApiServiceTests : BaseTest
{
    private ApiService _apiService;
    private readonly Mock<IClock> _clock;

    public ApiServiceTests()
    {
        _clock = GetClock();
    }

    [Fact]
    public void GetImaByDate_MoreIma()
    {
        // Arrange
        var date = "2022-01-01 01:01:01";
        var cache = new ImaCache();

        var imak = new List<ImaModel>();
        for (int i = 0; i < 11; i++)
        {
            imak.Add(new ImaModel(Guid.NewGuid(), DateTime.Now, "cim", "1", "szoveg"));
        }
        cache.Imak = imak;
        
        _apiService = new ApiService(_clock.Object);

        // Act
        var result = _apiService.GetImaByDate(date, cache);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetHumorAndAjandek_NoTodayContent()
    {
        // Arrange
        var cache = new MainCache();
        var id = Guid.NewGuid();

        _apiService = new ApiService(_clock.Object);

        cache.ContentDetailsModels = new List<ContentDetailsModel>
        {
            new(id, DateTime.Now.AddDays(-10), "cim", "audiofile", (int)Forras.humor, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        };

        // Act
        var result = _apiService.GetContentByTipusAndNid((int)Forras.humor, id, cache);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetHumorAndAjandek_NoContent()
    {
        // Arrange
        var cache = new MainCache();
        var id = Guid.NewGuid();

        _apiService = new ApiService(_clock.Object);

        cache.ContentDetailsModels = new List<ContentDetailsModel>
        {
            new(id, DateTime.Now, "cim", "audiofile", (int)Forras.ajanlatweb, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        };

        // Act
        var result = _apiService.GetContentByTipusAndNid((int)Forras.humor, id, cache);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetRadioBySource_Happy()
    {
        // Arrange
        var cache = new BookRadioSysCache();
        var id = "katolikusradio";

        _apiService = new ApiService(_clock.Object);

        cache.RadioMusor = new List<RadioMusorModel>
        {
            new(Guid.NewGuid(), "katolikusradio", "musor", "inserted")
        };

        // Act
        var result = _apiService.GetRadioBySource(id, cache);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetVideoForWebPage_Happy()
    {
        // Arrange
        var cache = new VideoCache();
        var id = "channel";

        _apiService = new ApiService(_clock.Object);

        cache.Video = new List<VideoModel>
        {
            new(Guid.NewGuid(), "img","videoLink", "cim", "date", "forras","channel", DateTime.Now)
        };

        // Act
        var result = _apiService.GetVideoForWebPage(id, 0, cache);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData((int)Forras.b777)]
    [InlineData((int)Forras.pio)]
    public void GetContentByTipusAndId_Happy(int tipus)
    {
        // Arrange
        var cache = new MainCache();
        var cacheBook = new BookRadioSysCache();
        cache.Egyeb = new List<ContentDetailsModel>
        {
            new(Guid.NewGuid(), DateTime.MinValue, "cim", string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        };

        cache.ContentDetailsModels = new List<ContentDetailsModel>
        {
            new(Guid.NewGuid(), DateTime.MinValue, "cim", string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        };

        _apiService = new ApiService(_clock.Object);

        // Act
        var result = _apiService.GetContentByTipusAndId(tipus, Guid.NewGuid(), cacheBook, cache);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData((int)Forras.b777)]
    [InlineData((int)Forras.pio)]
    public void GetContentByTipusAndId_NoContent(int tipus)
    {
        // Arrange
        var cache = new MainCache();
        var cacheBook = new BookRadioSysCache();
        cache.Egyeb = new List<ContentDetailsModel>();
        cache.ContentDetailsModels = new List<ContentDetailsModel>();

        _apiService = new ApiService(_clock.Object);

        // Act
        var result = _apiService.GetContentByTipusAndId(tipus, Guid.NewGuid(), cacheBook, cache);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
