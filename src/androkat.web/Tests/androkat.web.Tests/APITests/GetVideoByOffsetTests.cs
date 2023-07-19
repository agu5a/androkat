using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.APITests;

public class GetVideoByOffsetTests : BaseTest
{
    [TestCase(-1)]
    [TestCase(51)]
    public void API_GetVideoByOffset_V2_BadRequest(int offset)
    {
        var api = new data.Controllers.V2.Api(null, GetMapper());
        var res = api.GetVideoByOffsetV2(offset);
        dynamic s = res.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Test]
    public void API_GetVideoByOffset_NoCache_V1()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        var cacheService = new Mock<ICacheService>();
        cacheService.Setup(s => s.VideoCacheFillUp()).Returns((new VideoCache
        {
            Video = new List<VideoModel>
            {
                new VideoModel(Guid.NewGuid(), "img", "vlink", "cim", DateTime.Now.ToString("yyyy-MM-dd"), "forras", "cId", DateTime.Now)
            },
            Inserted = DateTime.Now
        }));

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, GetIMemoryCache(), cacheService.Object);

        var apiV1 = new data.Controllers.V2.Api(dec, GetMapper());
        ActionResult<List<VideoResponse>> resV1 = apiV1.GetVideoByOffsetV2(0);
        dynamic sV1 = resV1.Result;

        var videoResponse = ((IReadOnlyCollection<VideoResponse>)sV1.Value).First();
        Assert.That(videoResponse.Cim, Is.EqualTo("cim"));
        Assert.That(videoResponse.VideoLink, Is.EqualTo("vlink"));
        Assert.That(videoResponse.Img, Is.EqualTo("img"));
        Assert.That(videoResponse.Forras, Is.EqualTo("forras"));
        Assert.That(videoResponse.ChannelId, Is.EqualTo("cId"));
        Assert.That(videoResponse.ChannelId, Is.EqualTo("cId"));
    }

    [Test]
    public void API_GetVideoByOffset_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new List<VideoResponse> { new VideoResponse { Cim = "cim" } };
        var cache = GetIMemoryCache();
        _ = cache.Set("VideoResponseCacheKey_0", result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        var api = new data.Controllers.V2.Api(dec, GetMapper());
        ActionResult<List<VideoResponse>> res = api.GetVideoByOffsetV2(0);
        dynamic s = res.Result;

        Assert.That(((IReadOnlyCollection<VideoResponse>)s.Value).First().Cim, Is.EqualTo("cim"));
    }

    [Test]
    public void API_GetVideoByOffset_V2_Zero()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new VideoCache { Video = new List<VideoModel>() };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.VideoCacheKey.ToString(), result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        var api = new data.Controllers.V2.Api(dec, GetMapper());
        ActionResult<List<VideoResponse>> res = api.GetVideoByOffsetV2(0);
        dynamic s = res.Result;

        Assert.That(((IReadOnlyCollection<VideoResponse>)s.Value).Count, Is.EqualTo(0));
    }
}