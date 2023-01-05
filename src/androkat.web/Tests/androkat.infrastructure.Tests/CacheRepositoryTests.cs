using androkat.application.Interfaces;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class CacheRepositoryTests : BaseTest
{
    [Test]
    public void GetActualMaiSzent_Ma_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using (var context = new AndrokatContext(GetDbContextOptions()))
        {
            var entity = new Maiszent
            {
                Datum = "02-03"
            };
            context.MaiSzent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetMaiSzentToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }
    }

    [Test]
    public void GetActualMaiSzent_Tegnap_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using (var context = new AndrokatContext(GetDbContextOptions()))
        {
            var entity = new Maiszent
            {
                Datum = "02-02"
            };
            context.MaiSzent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetMaiSzentToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }
    }

    [Test]
    public void GetActualMaiSzent_ElozoHonap_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using (var context = new AndrokatContext(GetDbContextOptions()))
        {
            var entity = new Maiszent
            {
                Datum = "01-31"
            };
            context.MaiSzent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetMaiSzentToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }
    }

    private static Mock<IClock> GetToday()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));
        return clock;
    }

    //[Test]
    //public void GetContentDetailsModel_Happy()
    //{
    //    var clock = new Mock<IClock>();
    //    clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

    //    var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
    //    var mapper = config.CreateMapper();

    //    var loggerRepo = new Mock<ILogger<CacheRepository>>();

    //    var logger = new Mock<ILogger<ContentMetaDataService>>();
    //    var service = new ContentMetaDataService(logger.Object);
    //    var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

    //    var contentMetaDataModels = Options.Create(new AndrokatConfiguration
    //    {
    //        ContentMetaDataList = metaDataList
    //    });

    //    using var context = new SQLiteContext(GetDbContextOptions());

    //    var guid = Guid.NewGuid();
    //    var _fixture = new Fixture();
    //    var content = _fixture.Create<Napiolvaso>();
    //    content.Nid = guid;
    //    content.Tipus = (int)Forras.papaitwitter;
    //    content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";

    //    context.Content.Add(content);
    //    context.SaveChanges();

    //    var repository = new CacheRepository(context, loggerRepo.Object, clock.Object, mapper);
    //    var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, clock.Object);
    //    var contentService = new ContentService(GetIMemoryCache(), mapper, cacheService, contentMetaDataModels);        

    //    var result = contentService.GetContentDetailsModel(new int[] { (int)Forras.papaitwitter, (int)Forras.advent, (int)Forras.bojte }).ToList();

    //    result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
    //    result[0].MetaData.TipusNev.Should().Be("Ferenc pápa twitter üzenete");
    //    result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
    //    result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
    //    result.Count.Should().Be(1);
    //}
}