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
    public void API_GetVideoByOffset_V1_BadRequest(int offset)
    {
        var apiV1 = new data.Controllers.Api(null);
        var resV1 = apiV1.GetVideoByOffset(offset);
        dynamic s = resV1.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Test]
    public void API_GetVideoByOffset_V1_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new List<VideoResponse> { new VideoResponse { Cim = "cim" } };
        var cache = GetIMemoryCache();
        _ = cache.Set("VideoResponseCacheKey_0", result);

        var service = new ApiService(null, cache, clock.Object);

        var apiV1 = new data.Controllers.Api(service);
        ActionResult<List<VideoResponse>> resV1 = apiV1.GetVideoByOffset(0);
        dynamic sV1 = resV1.Result;

        Assert.That(((IReadOnlyCollection<VideoResponse>)sV1.Value).First().Cim, Is.EqualTo("cim"));
    }

    [Test]
    public void API_GetVideoByOffset_V1_V2_Zero()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new VideoCache { Video = new List<VideoModel>() };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.VideoCacheKey.ToString(), result);

        var service = new ApiService(null, cache, clock.Object);

        var apiV1 = new data.Controllers.Api(service);
        ActionResult<List<VideoResponse>> resV1 = apiV1.GetVideoByOffset(0);
        dynamic sV1 = resV1.Result;

        Assert.That(((IReadOnlyCollection<VideoResponse>)sV1.Value).Count, Is.EqualTo(0));
    }
}