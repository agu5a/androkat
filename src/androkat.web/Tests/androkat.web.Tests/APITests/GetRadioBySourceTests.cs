using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using androkat.web.Controllers.V2;

namespace androkat.web.Tests.APITests;

public class GetRadioBySourceTests : BaseTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("wrong_radio")]
    public void API_GetRadioBySource_V2_BadRequest(string s)
    {
        var api = new Api(null);
        var res = api.GetRadioBySourceV2(s);
        dynamic s2 = res.Result;
        string result = s2.Value;
        result.Should().Be("Hiba");
    }

    [Fact]
    public void API_GetRadioBySource_V1_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new List<RadioMusorResponse> { new() { Musor = "musor" } };
        var cache = GetIMemoryCache();
        _ = cache.Set("RadioResponseCacheKey_katolikushu", result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var api = new Api(dec);
        ActionResult<IEnumerable<RadioMusorResponse>> res = api.GetRadioBySourceV2("katolikushu");
        dynamic s = res.Result;

        ((IEnumerable<RadioMusorResponse>)s.Value).First().Musor.Should().Be("musor");
    }

    [Fact]
    public void API_GetRadioBySource_V1_V2_wrong_input()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new BookRadioSysCache
        {
            RadioMusor = new List<RadioMusorModel>
            {
                new(Guid.NewGuid(), "katolikushu", "musor", DateTime.Now.ToString("yyyy-MM-dd"))
            }
        };
        var memoryCache = new Mock<IMemoryCache>();
        memoryCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out result)).Returns(true);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, memoryCache.Object, null);
        var api = new Api(dec);
        var res = api.GetRadioBySourceV2("kat");
        dynamic s = res.Result;

        (s.Value as string).Should().Be("Hiba");
        (s.StatusCode as int?).Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
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
        var api = new Api(dec);
        var res = api.GetRadioBySourceV2("katolikushu");
        dynamic s = res.Result;

        ((IEnumerable<RadioMusorResponse>)s.Value).Count().Should().Be(0);
    }
}