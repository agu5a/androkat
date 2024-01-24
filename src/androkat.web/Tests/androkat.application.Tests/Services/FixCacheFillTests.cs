using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Configuration;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class FixCacheFillTests : BaseTest
{
    [Test]
    public void FixCacheFillUpSelectMaiSzent1()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Maiszent
        {
            Datum = "02-01"
        };
        context.MaiSzent.Add(entity);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        MainCache res = cacheService.MainCacheFillUp();

        Assert.That(res.ContentDetailsModels.Where(w => w.Tipus == 21).Count(), Is.EqualTo(1));
    }

    [Test]
    public void FixCacheFillUpSelectMaiSzent2()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Maiszent
        {
            Datum = "01-01"
        };
        context.MaiSzent.Add(entity);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        MainCache res = cacheService.MainCacheFillUp();

        Assert.That(res.ContentDetailsModels.Where(w => w.Tipus == 21).Count(), Is.EqualTo(1));
    }

    [Test]
    public void FixCacheFillUpSelectMaiSzent3()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var mock = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Maiszent
        {
            Datum = "01-01"
        };
        context.MaiSzent.Add(entity);
        context.SaveChanges();

        var entity2 = new Maiszent
        {
            Datum = "01-02"
        };
        context.MaiSzent.Add(entity2);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new CacheRepository(context, mock.Object, clock.Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.MainCacheFillUp();

        Assert.That(res.ContentDetailsModels.Where(w => w.Tipus == 21).Count(), Is.EqualTo(1));
        Assert.That(res.ContentDetailsModels.Where(w => w.Tipus == 21).FirstOrDefault().Fulldatum.ToString("yyyy-MM-dd HH:mm:ss"),
            Is.EqualTo(DateTime.Now.ToString("yyyy") + "-01-02 00:00:00"));
    }
}