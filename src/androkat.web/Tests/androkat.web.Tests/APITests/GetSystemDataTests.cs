using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using androkat.web.Controllers.V2;

namespace androkat.web.Tests.APITests;

public class GetSystemDataTests : BaseTest
{
    [Test]
    public void API_GetSystemData_V2()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new BookRadioSysCache { SystemData = new List<SystemInfoModel> { new(1, "key1", "value1") } };
        var memoryCache = new Mock<IMemoryCache>();
        memoryCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out result)).Returns(true);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, memoryCache.Object, null);
        var api = new Api(dec);
        ActionResult<IEnumerable<SystemDataResponse>> res = api.GetSystemDataV2();
        dynamic s = res.Result;

        Assert.That(((IEnumerable<SystemDataResponse>)s.Value).First().Key, Is.EqualTo("key1"));
        Assert.That(((IEnumerable<SystemDataResponse>)s.Value).First().Value, Is.EqualTo("value1"));
    }

    [Test]
    public void API_GetSystemData_V2_Zero()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        object result = new BookRadioSysCache
        {
            SystemData = new List<SystemInfoModel>(),
            Books = new List<ContentDetailsModel>(),
            RadioMusor = new List<RadioMusorModel>()
        };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.BookRadioSysCacheKey.ToString(), result);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var api = new Api(dec);
        ActionResult<IEnumerable<SystemDataResponse>> res = api.GetSystemDataV2();
        dynamic s = res.Result;

        Assert.That(((IEnumerable<SystemDataResponse>)s.Value).Count, Is.EqualTo(0));
    }
}