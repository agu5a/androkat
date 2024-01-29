using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using androkat.web.Controllers.V2;

namespace androkat.web.Tests.APITests;

public class GetVideoByOffsetTests : BaseTest
{
    [Theory]
    [InlineData(-1)]
    [InlineData(51)]
    public void API_GetVideoByOffset_V2_BadRequest(int offset)
    {
        var api = new Api(null);
        var res = api.GetVideoByOffsetV2(offset);
        dynamic s = res.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Fact]
    public void API_GetVideoByOffset_NoCache_V1()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        var cacheService = new Mock<ICacheService>();
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        cacheService.Setup(s => s.VideoCacheFillUp()).Returns((new VideoCache
        {
            Video = new List<VideoModel>
            {
                new(Guid.NewGuid(), "img", "vlink", "cim", date, "forras", "cId", DateTime.Now)
            },
            Inserted = DateTime.Now
        }));

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, GetIMemoryCache(), cacheService.Object);

        var apiV2 = new Api(dec);
        ActionResult<List<VideoResponse>> resV1 = apiV2.GetVideoByOffsetV2(0);
        dynamic sV1 = resV1.Result;

        var videoResponse = ((IReadOnlyCollection<VideoResponse>)sV1.Value).First();
        videoResponse.Cim.Should().Be("cim");
        videoResponse.VideoLink.Should().Be("vlink");
        videoResponse.Img.Should().Be("img");
        videoResponse.Date.Should().Be(date);
        videoResponse.Forras.Should().Be("forras");
        videoResponse.ChannelId.Should().Be("cId");
        videoResponse.ChannelId.Should().Be("cId");
    }

    [Fact]
    public void API_GetVideoByOffset_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new List<VideoResponse> { new() { Cim = "cim" } };
        var cache = GetIMemoryCache();
        _ = cache.Set("VideoResponseCacheKey_0", result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        var api = new Api(dec);
        ActionResult<List<VideoResponse>> res = api.GetVideoByOffsetV2(0);
        dynamic s = res.Result;

        ((IReadOnlyCollection<VideoResponse>)s.Value).First().Cim.Should().Be("cim");
    }

    [Fact]
    public void API_GetVideoByOffset_V2_Zero()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new VideoCache { Video = new List<VideoModel>() };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.VideoCacheKey.ToString(), result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        var api = new Api(dec);
        ActionResult<List<VideoResponse>> res = api.GetVideoByOffsetV2(0);
        dynamic s = res.Result;

        ((IReadOnlyCollection<VideoResponse>)s.Value).Count.Should().Be(0);
    }
}