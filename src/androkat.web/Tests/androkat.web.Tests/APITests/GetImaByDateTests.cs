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

public class GetImaByDateTests : BaseTest
{
    [TestCase("")]
    [TestCase(null)]
    [TestCase("20011-01-01")] //wrong date, plus char
    [TestCase("2022-0-02 10:20:20")] //wrong date, less 1 char
    public void API_GetImaByDate_V2_BadRequest(string date)
    {
        var api = new data.Controllers.V2.Api(null, GetMapper());
        var res = api.GetImaByDateV2(date);
        dynamic s = res.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Test]
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
        var api = new data.Controllers.V2.Api(dec, GetMapper());
        ActionResult<ImaResponse> res = api.GetImaByDateV2(now.ToString("yyyy-MM-dd HH:mm:dd"));
        dynamic s = res.Result;

        Assert.That(((ImaResponse)s.Value).Imak.First().Cim, Is.EqualTo("cim1"));
        Assert.That(((ImaResponse)s.Value).Imak.Count, Is.EqualTo(1));
    }

    [Test]
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
        var api = new data.Controllers.V2.Api(dec, GetMapper());
        ActionResult<ImaResponse> res = api.GetImaByDateV2(now.ToString("yyyy-MM-dd HH:mm:dd"));
        dynamic s = res.Result;

        Assert.That(((ImaResponse)s.Value).Imak.Count, Is.EqualTo(0));
    }
}