using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using androkat.infrastructure.Mapper;
using androkat.web.Controllers.V3;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.APITests.V3;

public class GetContentsByTipusAndIdTests : BaseTest
{
    public GetContentsByTipusAndIdTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
    }

    [Theory]
    [InlineData(1, "x281cd115128911ea8aa1cbeb38570c35")] //wrong guid, plus char
    [InlineData(1, "81cd115-1289-11ea-8aa1-cbeb38570c35")] //wrong guid, less 1 char
    [InlineData((int)Forras.book, "x281cd115128911ea8aa1cbeb38570c35")] //wrong guid, plus char
    [InlineData((int)Forras.b777, "81cd115-1289-11ea-8aa1-cbeb38570c35")] //wrong guid, less 1 char
    public void API_GetContentsByTipusAndId_BadRequest(int tipus, string id)
    {
        var apiV3 = new Api(null);
        var resV1 = apiV3.GetContentsByTipusAndId(tipus, id);
        dynamic s = resV1.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Theory]
    [InlineData((int)Forras.book, "")]
    [InlineData((int)Forras.b777, "")]
    [InlineData((int)Forras.book, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.b777, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    public void API_GetEgyebOlvasnivaloByForrasAndNid_Book(int tipus, string id)
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var yesterday = now.AddDays(-1).DateTime;
        var beforeOfyesterday = now.AddDays(-2).DateTime;
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object bookRadioSysCache = new BookRadioSysCache
        {
            Books = new List<ContentDetailsModel>
                {
                new(Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim1", string.Empty, tipus, yesterday, string.Empty, string.Empty, string.Empty, string.Empty),
                new(Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim2", string.Empty, tipus, beforeOfyesterday, string.Empty, string.Empty, string.Empty, string.Empty)                    
                }
        };
        object mainCache = new MainCache
        {
            Egyeb = new List<ContentDetailsModel>
                {
                    new(Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim1", string.Empty, tipus, yesterday, string.Empty, string.Empty, string.Empty, string.Empty),                 
                    new(Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim2", string.Empty, tipus, beforeOfyesterday, string.Empty, string.Empty, string.Empty, string.Empty)                    
                }
        };
        var cache = GetIMemoryCache();
        _ = cache.Set("BookRadioSysCacheKey", bookRadioSysCache);
        _ = cache.Set("MainCacheKey", mainCache);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var apiV3 = new Api(dec);

        var res = apiV3.GetContentsByTipusAndId(tipus, id);
        dynamic s = res.Result;

        if (string.IsNullOrWhiteSpace(id))
        {
            ((IEnumerable<ContentResponse>)s.Value).First().Cim.Should().Be("cim1");
            ((IEnumerable<ContentResponse>)s.Value).First().KulsoLink.Should().BeEmpty();
            ((IEnumerable<ContentResponse>)s.Value).ElementAt(1).Cim.Should().Be("cim2");
            ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(2);
        }
        else
        {
            ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(0);
        }
    }

    [Fact]
    public void API_GetEgyebOlvasnivaloByForrasAndNid_Book_V1_V2_Zero()
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object bookRadioSysCache = new BookRadioSysCache { Books = new List<ContentDetailsModel>() };
        object egyebCache = new MainCache { Egyeb = new List<ContentDetailsModel>() };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.MainCacheKey.ToString(), egyebCache);
        _ = cache.Set(CacheKey.BookRadioSysCacheKey.ToString(), bookRadioSysCache);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var apiV3 = new Api(dec);
        var res = apiV3.GetContentsByTipusAndId((int)Forras.book, null);
        dynamic s = res.Result;
        ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(0);
    }

    [Fact]
    public void API_GetEgyebOlvasnivaloByForrasAndNid_B777_V1_V2_Zero()
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object bookRadioSysCache = new BookRadioSysCache { Books = new List<ContentDetailsModel>() };
        object egyebCache = new MainCache { Egyeb = new List<ContentDetailsModel>() };
        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.MainCacheKey.ToString(), egyebCache);
        _ = cache.Set(CacheKey.BookRadioSysCacheKey.ToString(), bookRadioSysCache);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var apiV3 = new Api(dec);
        ActionResult<IEnumerable<ContentResponse>> res = apiV3.GetContentsByTipusAndId((int)Forras.b777, null);
        dynamic s = res.Result;
        ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(0);
    }

    [Theory]
    [InlineData((int)Forras.ajanlatweb, "")]
    [InlineData((int)Forras.humor, "")]
    [InlineData((int)Forras.maiszent, "")]
    [InlineData(2, "")]
    [InlineData((int)Forras.ajanlatweb, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.humor, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData(2, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.audiotaize, "")]
    public void API_GetContentByTipusAndNidV2(int tipus, string nid)
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object result = new MainCache
        {
            ContentDetailsModels = new List<ContentDetailsModel>
                {
                    new(Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"), now.DateTime, "cim1",
                    tipus == (int)Forras.audiotaize ? "audiofile" : "idezet", tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty),                    
                    new(Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim2",
                    tipus == (int)Forras.audiotaize ? "audiofile" : "idezet", tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
                }
        };
        object emptyResult = new MainCache { ContentDetailsModels = new List<ContentDetailsModel>() };
        var cache = GetIMemoryCache();

        _ = cache.Set("MainCacheKey", result);
        _ = cache.Set("FixCacheKey", emptyResult);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var apiV3 = new Api(dec);
        ActionResult<IEnumerable<ContentResponse>> res = apiV3.GetContentsByTipusAndId(tipus, nid);
        dynamic s = res.Result;

        if (string.IsNullOrWhiteSpace(nid))
        {
            ((IEnumerable<ContentResponse>)s.Value).First().Cim.Should().Be("cim1");
            ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(tipus == 2 || tipus == (int)Forras.audiotaize || tipus == (int)Forras.maiszent ? 2 : 1);
        }
        else
        {
            ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(0);
        }

        if (tipus == (int)Forras.audiotaize)
        {
            ((IEnumerable<ContentResponse>)s.Value).First().Idezet.Should().Be("audiofile");
            ((IEnumerable<ContentResponse>)s.Value).ElementAt(1).Idezet.Should().Be("audiofile");
        }
    }

    [Theory]
    [InlineData(28)]
    public void API_GetContentsByTipusAndId_Zero(int f)
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object mainCache = new MainCache { ContentDetailsModels = new List<ContentDetailsModel>(), Egyeb = new List<ContentDetailsModel>() };

        var cache = GetIMemoryCache();
        _ = cache.Set(CacheKey.MainCacheKey.ToString(), mainCache);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var apiV3 = new Api(dec);
        ActionResult<IEnumerable<ContentResponse>> res = apiV3.GetContentsByTipusAndId(f, null);
        dynamic s = res.Result;
        ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(0);
    }

    [Theory]
    [InlineData(28, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData(28, "")]
    public void API_GetContentsByTipusAndId(int f, string nid)
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");
        //var yesterday = now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
        //var beforeOfyesterday = now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss");
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        object mainCache = new MainCache
        {
            ContentDetailsModels = new List<ContentDetailsModel>
                {
                    new(Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim1", "https://valami", 28, now.DateTime.AddDays(-1), string.Empty, string.Empty, string.Empty, string.Empty),
                    
                    new(Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"), now.DateTime, "cim2", "https://valami2", 28, now.DateTime, string.Empty, string.Empty, string.Empty, string.Empty)
                }
        };

        object fixCache = new MainCache { ContentDetailsModels = new List<ContentDetailsModel>(), Egyeb = null };

        var cache = GetIMemoryCache();
        _ = cache.Set("MainCacheKey", mainCache);
        _ = cache.Set("FixCacheKey", fixCache);

        var service = new ApiService(clock.Object);
        var dec = new ApiServiceCacheDecorate(service, cache, null);
        var apiV3 = new Api(dec);
        var res = apiV3.GetContentsByTipusAndId(f, nid);
        dynamic s = res.Result;

        if (string.IsNullOrWhiteSpace(nid))
        {
            ((IEnumerable<ContentResponse>)s.Value).First().Cim.Should().Be("cim1");
            ((IEnumerable<ContentResponse>)s.Value).First().Idezet.Should().Be("https://valami");
            ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(2);
        }
        else
        {
            ((IEnumerable<ContentResponse>)s.Value).Count().Should().Be(0);
        }
    }
}
