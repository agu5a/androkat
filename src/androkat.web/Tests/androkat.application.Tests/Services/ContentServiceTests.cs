using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class ContentServiceTests : BaseTest
{
    private Mock<IClock> GetClock()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));
        return clock;
    }

    private IOptions<AndrokatConfiguration> GetAndrokatConfiguration()
    {
        var logger = new Mock<ILogger<ContentMetaDataService>>();
        var service = new ContentMetaDataService(logger.Object);
        var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = metaDataList
        });

        return contentMetaDataModels;
    }

    [Test]
    public void GetHome_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var content = _fixture.Create<Napiolvaso>();
        content.Cim = "Twitter cím";
        content.Nid = guid;
        content.Tipus = (int)Forras.papaitwitter;
        content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";

        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHome().ToList();

        result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
        result[0].MetaData.TipusNev.Should().Be("Ferenc pápa twitter üzenete");
        result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[0].ContentDetails.Cim.Should().Be("Twitter cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetAjanlat_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var content = _fixture.Create<Napiolvaso>();
        content.Cim = "Ajánlat cím";
        content.Nid = guid;
        content.Tipus = (int)Forras.ajanlatweb;
        content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";

        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetAjanlat().ToList();

        result[0].MetaData.Image.Should().Be("images/gift.png");
        result[0].MetaData.TipusNev.Should().Be("AJÁNDéKOZZ KÖNYVET");
        result[0].MetaData.TipusId.Should().Be(Forras.ajanlatweb);
        result[0].ContentDetails.Cim.Should().Be("Ajánlat cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.ajanlatweb);
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetAjanlat_Details_Happy()
    {
        var nid = Guid.NewGuid();
        var tipus = (int)Forras.ajanlatweb;

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Napiolvaso>();
        content.Cim = "Ajánlat cím";
        content.Nid = nid;
        content.Tipus = tipus;
        content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";

        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetContentDetailsModelByNid(nid, tipus);

        result.MetaData.Image.Should().Be("images/gift.png");
        result.MetaData.TipusNev.Should().Be("AJÁNDéKOZZ KÖNYVET");
        result.MetaData.TipusId.Should().Be(Forras.ajanlatweb);
        result.ContentDetails.Cim.Should().Be("Ajánlat cím");
        result.ContentDetails.Tipus.Should().Be((int)Forras.ajanlatweb);
    }

    [Test]
    public void GetSzentek_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var fixContent = _fixture.Create<FixContent>();
        fixContent.Cim = "Pio cím";
        fixContent.Nid = guid;
        fixContent.Tipus = (int)Forras.pio;
        fixContent.Datum = "02-03";

        context.FixContent.Add(fixContent);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetSzentek().ToList();

        result[0].MetaData.Image.Should().Be("images/pio.png");
        result[0].MetaData.TipusNev.Should().Be("Pio atya breviáriuma");
        result[0].MetaData.TipusId.Should().Be(Forras.pio);
        result[0].ContentDetails.Cim.Should().Be("Pio cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.pio);
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetImaPage_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cacheService = new CacheService(new Mock<ICacheRepository>().Object, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetImaPage(string.Empty).ToList();

        result[0].ContentDetails.Cim.Should().Be("Ima Cím");
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetImaDetailsPage_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cacheService = new CacheService(new Mock<ICacheRepository>().Object, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetImaById(Guid.Empty);

        result.Cim.Should().Be("Ima Cím");
        result.Csoport.Should().Be("0");
        result.Szoveg.Should().Be("Szöveg");
    }

    [Test]
    public void GetAudio_Happy()
    {
        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");

        var cacheService = GetCacheService(
            new List<ContentModel>
            {
                new ContentModel
                {
                    ContentDetails = new ContentDetailsModel
                    {
                        Cim = "Audio cím", Tipus = 60, Fulldatum = now.DateTime, Nid = Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"),
                        Idezet = "audiofile",
                        FileUrl = ""
                    }
                }
            });

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var contentService = new ContentService(GetIMemoryCache(), cacheService.Object, GetAndrokatConfiguration());

        var result = contentService.GetAudio().ToList();

        result[0].Cim.Should().Be("Audio cím");
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetVideoSource_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cacheService = new CacheService(new Mock<ICacheRepository>().Object, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetVideoSourcePage().ToList();

        result[0].ChannelId.Should().Be("UCF3mEbdkhZwjQE8reJHm4sg");
        result.Count.Should().Be(1);
    }

    [TestCase((int)Forras.b777)]
    [TestCase(0)]
    public void GetBlog_Happy(int tipus)
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var content = _fixture.Create<Napiolvaso>();
        content.Cim = "Blog cím";
        content.Nid = guid;
        content.Tipus = (int)Forras.b777;
        content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";
        content.Inserted = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-02-03");

        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetBlog(tipus).ToList();

        result[0].ContentDetails.Cim.Should().Be("Blog cím");
        result.Count.Should().Be(1);
    }

    [TestCase((int)Forras.kurir)]
    [TestCase(0)]
    public void GetHirek_Happy(int tipus)
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var content = new Napiolvaso
        {
            Cim = "Hír cím",
            Nid = guid,
            Tipus = (int)Forras.kurir,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03",
            Inserted = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-02-03"),
            Idezet = "Idézet",
            //KulsoLink = "KulsoLink"
        };

        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHirek(tipus).ToList();

        result[0].ContentDetails.Cim.Should().Be("Hír cím");
        result[0].ContentDetails.Idezet.Should().Be("Idézet");
        result[0].ContentDetails.KulsoLink.Should().Be("KulsoLink");
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetHumor_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var fixContent = _fixture.Create<FixContent>();
        fixContent.Cim = "Humor cím";
        fixContent.Nid = guid;
        fixContent.Tipus = (int)Forras.humor;
        fixContent.Datum = "02-03";

        context.FixContent.Add(fixContent);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHumor().ToList();

        result[0].MetaData.Image.Should().Be("images/smile.jpg");
        result[0].MetaData.TipusNev.Should().Be("Humor");
        result[0].MetaData.TipusId.Should().Be(Forras.humor);
        result[0].ContentDetails.Cim.Should().Be("Humor cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.humor);
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetRadioPage_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cacheService = new CacheService(new Mock<ICacheRepository>().Object, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetRadioPage().ToList();

        result[0].Name.Should().Be("szentistvan");
        result[0].Url.Should().Be("http://online.szentistvanradio.hu:7000/adas");
        result.Count.Should().Be(9);
    }

    [Test]
    public void GetSzent_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var maiszent = _fixture.Create<Maiszent>();
        maiszent.Cim = "Mai szent cím";
        maiszent.Nid = guid;
        maiszent.Idezet = "Idézet";
        maiszent.Datum = "02-03";

        context.MaiSzent.Add(maiszent);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetSzent().ToList();

        result[0].MetaData.Image.Should().Be("images/katolikus_hu.jpg");
        result[0].MetaData.TipusNev.Should().Be("Mai Szent");
        result[0].MetaData.TipusId.Should().Be(Forras.maiszent);
        result[0].ContentDetails.Cim.Should().Be("Mai szent cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.maiszent);
        result.Count.Should().Be(1);
    }
}