﻿using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using androkat.web.Controllers.V2;

namespace androkat.web.Tests.APITests;

public class GetVideoForWebPageTests : BaseTest
{
    [Theory]
    [InlineData("UCRn003-qzC5GOQVPyJSkgnA", -1)]
    [InlineData("UCRn003-qzC5GOQVPyJSkgnA", 51)]
    [InlineData("UCRn003-qzC5GOQVPyJ", 1)] //less than 20
    [InlineData("UCRn003-qzC5GOQVPyJSkgnA1111111", 50)] //more than 30
    public void API_GetVideoForWebPage_V2_BadRequest(string f, int offset)
    {
        var apiV2 = new Api(null);
        var resV1 = apiV2.GetVideoForWebPage(f, offset);
        dynamic s = resV1.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Fact]
    public void API_GetVideoForWebPage_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));
        object result = new VideoCache
        {
            Video = new List<VideoModel>
            {
                new(Guid.Empty, string.Empty, "watch?v=", "cim", string.Empty, string.Empty, string.Empty, DateTime.MinValue)
            }
        };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.VideoCacheKey.ToString(), result);
        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        var apiV2 = new Api(dec);
        ActionResult<string> resV1 = apiV2.GetVideoForWebPage("", 0);
        dynamic sV1 = resV1.Result;

        string actual = sV1.Value.ToString();
        actual.Should().Be("<div class='col mb-1'><div class=\"videoBox p-3 border bg-light\" style=\"border-radius: 0.25rem;\"><h5><a href=\"watch?v=\" target =\"_blank\">cim</a></h5><div><strong>Forrás</strong>: </div><div></div><div class=\"video-container\" ><a href=\"watch?v=\" target =\"_blank\"><img src=\"\"></a></div></div></div>");
    }

    [Fact]
    public void API_GetVideoForWebPage_V2_Exception()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new VideoCache { Video = null };
        var memoryCache = new Mock<IMemoryCache>();
        memoryCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out result)).Returns(true);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, memoryCache.Object, null);
        var apiV2 = new Api(dec);
        Assert.Throws<ArgumentNullException>(() => apiV2.GetVideoForWebPage("", 0));
    }
}