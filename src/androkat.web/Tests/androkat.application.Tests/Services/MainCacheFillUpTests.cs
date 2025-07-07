using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class MainCacheFillUpTests : BaseTest
{
    [Fact]
    public void MainCacheFillUp_GetHirekBlogokToCache_Happy_test()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

        var cacheRepository = new Mock<ICacheRepository>();
        cacheRepository.Setup(s => s.GetHirekBlogokToCache()).Returns(new List<ContentDetailsModel>
        {
            new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        });
        cacheRepository.Setup(s => s.GetHumorToCache()).Returns(new List<ContentDetailsModel>
        {
            new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        });
        cacheRepository.Setup(s => s.GetMaiSzentToCache()).Returns(new List<ContentDetailsModel>
        {
            new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        });
        cacheRepository.Setup(s => s.GetTodayFixContentToCache()).Returns(new List<ContentDetailsModel>
        {
            new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        });
        cacheRepository.Setup(s => s.GetContentDetailsModelToCache()).Returns(new List<ContentDetailsModel>
        {
            new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        });

        var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        res.Egyeb.Count.Should().Be(1);
        res.Inserted.ToString("yyyy-MM-dd").Should().Be("2012-01-03");
    }

    [Fact]
    public void MainCacheFillUp_GetHirekBlogokToCache_Throws_Exception()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

        var cacheRepository = new Mock<ICacheRepository>();
        cacheRepository.Setup(s => s.GetHirekBlogokToCache()).Throws<Exception>();

        var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);

        Action act = () => cacheService.MainCacheFillUp();
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void MainCacheFillUpNapiutravaloWeboldalTest()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = 15,
            FileUrl = string.Empty,
            Fulldatum = DateTimeOffset.Parse("2011-12-03T03:05:06").DateTime.ToString("yyyy-MM-dd"),
            Inserted = DateTimeOffset.Parse("2011-12-03T03:05:06").DateTime
        };
        context.Content.Add(entity);

        var entity2 = new Content
        {
            Tipus = 15,
            FileUrl = string.Empty,
            Fulldatum = DateTimeOffset.Parse("2011-12-04T03:05:06").DateTime.ToString("yyyy-MM-dd"),
            Inserted = DateTimeOffset.Parse("2011-12-04T03:05:06").DateTime
        };
        context.Content.Add(entity2);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        var napiutravalo = res.ContentDetailsModels.Where(w => w.Tipus == 15);

        napiutravalo.Count().Should().Be(2);
        napiutravalo.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be("2011-12-04");
    }

    /// <summary>
    /// Content.Where(w => w.tipus == tipus && w.fulldatum.ToString("yyyy-MM-dd") == date)
    /// </summary>
    [Fact]
    public void MainCacheFillUpFirstSelect()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = 60,
            FileUrl = string.Empty,
            Fulldatum = DateTimeOffset.Parse("2012-02-03T03:05:06").DateTime.ToString("yyyy-MM-dd")
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        res.ContentDetailsModels.Where(w => w.Tipus == 60).Count().Should().Be(1);
    }

    [Fact]
    public void MainCacheFillUpNoRecord()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var androkatRepository = new CacheRepository(context, mock.Object, clock.Object, null);
        var cacheService = new CacheService(androkatRepository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        res.ContentDetailsModels.Where(w => w.Tipus == 60).Count().Should().Be(0);
    }

    [Fact(DisplayName = "Where(w => w.tipus == tipus).OrderByDescending(o => o.fulldatum).Take(1)")]
    public void MainCacheFillUpLastSelect()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = 60,
            FileUrl = string.Empty,
            Fulldatum = DateTimeOffset.Parse("2012-02-01T03:05:06").DateTime.ToString("yyyy-MM-dd")
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        res.ContentDetailsModels.Where(w => w.Tipus == 60).Count().Should().Be(1);
    }

    [Theory]
    [InlineData(7, "2012-02-01T04:05:06", "2012-01-03T03:05:06", "2012-02-01", "2012-02-01 00:00:00")]//fokolare
    [InlineData(7, "2012-02-02T04:05:06", "2012-01-03T03:05:06", "2012-02-01", "2012-02-01 00:00:00")]//fokolare
    [InlineData(6, "2012-02-02T04:05:06", "2012-01-03T03:05:06", "2012-02-02", "2012-02-02 00:00:00")]
    [InlineData(6, "2012-02-02T04:05:06", "2012-01-03T03:05:06", "2012-02-01", "2012-02-01 00:00:00")]//nincs mai
    [InlineData(6, "2012-01-02T04:05:06", "2011-12-31T03:05:06", "2011-12-31", "2011-12-31 00:00:00")]//tavalyi
    public void GetContentByTipusAndNidV2Test(int tipus, string today, string insertedInDb, string datumInDb, string resultDatum)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(today));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Nid = Guid.NewGuid(),
            Tipus = tipus,
            Fulldatum = datumInDb,
            FileUrl = string.Empty,
            Inserted = DateTimeOffset.Parse(insertedInDb).DateTime
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        res.ContentDetailsModels.Where(w => w.Tipus == tipus).Count().Should().Be(1);
        res.ContentDetailsModels.Where(w => w.Tipus == tipus).First().Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be(resultDatum);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetContentByTipusAndNidV2_MaiIge_Test(bool twoItems)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-02-01T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        Content entity2 = null;
        var entity = new Content
        {
            Nid = Guid.NewGuid(),
            Tipus = 11,
            Fulldatum = "2012-02-01",
            FileUrl = string.Empty,
            Inserted = DateTimeOffset.Parse("2012-01-03T03:05:06").DateTime
        };
        context.Content.Add(entity);
        context.SaveChanges();

        if (twoItems)
        {
            entity2 = new Content
            {
                Nid = Guid.NewGuid(),
                Tipus = 11,
                Fulldatum = "2012-02-02",
                FileUrl = string.Empty,
                Inserted = DateTimeOffset.Parse("2012-01-03T03:05:06").DateTime
            };
            context.Content.Add(entity2);
            context.SaveChanges();
        }

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        res.ContentDetailsModels.Where(w => w.Tipus == 11).Count().Should().Be(twoItems ? 2 : 1);
    }
}