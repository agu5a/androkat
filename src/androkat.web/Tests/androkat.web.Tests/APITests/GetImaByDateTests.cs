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

public class GetImaByDateTests : BaseTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("20011-01-01")] //wrong date, plus char
    [InlineData("2022-0-02 10:20:20")] //wrong date, less 1 char
    public void API_GetImaByDate_V2_BadRequest(string date)
    {
        var api = new Api(null);
        var res = api.GetImaByDateV2(date);
        dynamic s = res.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Fact]
    public void API_GetImaByDate_V2()
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var clock = new Mock<application.Interfaces.IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object result = new ImaCache
        {
            Imak = new List<ImaModel>
            {
                new(Guid.NewGuid(), now.AddMinutes(1).DateTime, "cim1", "1", "")
            }
        };
        var cache = GetIMemoryCache();

        _ = cache.Set("ImaCacheKey", result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var api = new Api(dec);
        ActionResult<ImaResponse> res = api.GetImaByDateV2(now.ToString("yyyy-MM-dd HH:mm:dd"));
        dynamic s = res.Result;

        ((ImaResponse)s.Value).Imak.First().Cim.Should().Be("cim1");
        ((ImaResponse)s.Value).Imak.Count.Should().Be(1);
    }

    [Fact]
    public void API_GetImaByDate_V2_No_Result()
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var clock = new Mock<application.Interfaces.IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object result = new ImaCache
        {
            Imak = new List<ImaModel> { new(Guid.NewGuid(), now.AddMinutes(-1).DateTime, "cim1", "", "") }
        };
        var cache = GetIMemoryCache();

        _ = cache.Set(CacheKey.ImaCacheKey.ToString(), result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var api = new Api(dec);
        ActionResult<ImaResponse> res = api.GetImaByDateV2(now.ToString("yyyy-MM-dd HH:mm:dd"));
        dynamic s = res.Result;

        ((ImaResponse)s.Value).Imak.Count.Should().Be(0);
    }
}