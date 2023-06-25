using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.APITests;

public class GetRadioBySourceTests : BaseTest
{
    [TestCase("")]
    [TestCase(null)]
    [TestCase("wrong_radio")]
    public void API_GetRadioBySource_V1_BadRequest(string s)
    {
        data.Controllers.Api apiV1 = new data.Controllers.Api(null);
        var resV1 = apiV1.GetRadioBySource(s);
        dynamic s2 = resV1.Result;
        string result = s2.Value;
        result.Should().Be("Hiba");
    }

    [Test]
    public void API_GetRadioBySource_V1_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new List<RadioMusorResponse> { new RadioMusorResponse { Musor = "musor" } };
        var cache = GetIMemoryCache();
        _ = cache.Set("RadioResponseCacheKey_katolikushu", result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        data.Controllers.Api apiV1 = new data.Controllers.Api(dec);
        ActionResult<IEnumerable<RadioMusorResponse>> resV1 = apiV1.GetRadioBySource("katolikushu");
        dynamic sV1 = resV1.Result;

        Assert.That(((IEnumerable<RadioMusorResponse>)sV1.Value).First().Musor, Is.EqualTo("musor"));
    }

    [Test]
    public void API_GetRadioBySource_V1_V2_wrong_input()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new BookRadioSysCache
        {
            RadioMusor = new List<RadioMusorModel>
            {
                new RadioMusorModel (Guid.NewGuid(), "katolikushu", "musor", DateTime.Now.ToString("yyyy-MM-dd"))
            }
        };
        var memoryCache = new Mock<IMemoryCache>();
        memoryCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out result)).Returns(true);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, memoryCache.Object, null);

        data.Controllers.Api apiV1 = new data.Controllers.Api(dec);
        ActionResult<IEnumerable<RadioMusorResponse>> resV1 = apiV1.GetRadioBySource("kat");
        dynamic sV1 = resV1.Result;

        Assert.That(sV1.Value, Is.EqualTo("Hiba"));
        Assert.That(sV1.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public void API_GetRadioBySource_V1_V2_Zero()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new BookRadioSysCache
        {
            RadioMusor = new List<RadioMusorModel>(),
            Books = new List<ContentDetailsModel>(),
            SystemData = new List<SystemInfoModel>()
        };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.BookRadioSysCacheKey.ToString(), result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);

        data.Controllers.Api apiV1 = new data.Controllers.Api(dec);
        ActionResult<IEnumerable<RadioMusorResponse>> resV1 = apiV1.GetRadioBySource("katolikushu");
        dynamic sV1 = resV1.Result;

        Assert.That(((IEnumerable<RadioMusorResponse>)sV1.Value).Count, Is.EqualTo(0));
    }
}