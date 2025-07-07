using androkat.application.Service;
using androkat.application.Tests.Attributes;
using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace androkat.application.Tests.Services;

public class ContentServiceWithDbTests : BaseTest
{
    [Theory, AutoDataWithCustomData]
    public void GetHome_Happy(List<Content> contents)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        // Set both content items to papaitwitter type and add to context
        foreach (var content in contents)
        {
            content.Tipus = (int)Forras.papaitwitter;
            context.Content.Add(content);
        }
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHome().ToList();

        // Check that we have 2 items
        result.Count.Should().Be(2);

        // Check first content item
        result[0].MetaData.Image.Should().Be("images/pope.png");
        result[0].MetaData.TipusNev.Should().Be("Pápa X üzenetei");
        result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[0].ContentDetails.Cim.Should().Be("Cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);

        // Check second content item
        result[1].MetaData.Image.Should().Be("images/pope.png");
        result[1].MetaData.TipusNev.Should().Be("Pápa X üzenetei");
        result[1].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[1].ContentDetails.Cim.Should().Be("Cím");
        result[1].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
    }

    [Theory, AutoDataWithCustomData]
    public void GetAjanlat_Happy(Content content)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        content.Tipus = (int)Forras.ajanlatweb;
        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetAjanlat().ToList();

        result[0].MetaData.Image.Should().Be("images/gift.png");
        result[0].MetaData.TipusNev.Should().Be("AJÁNDéKOZZ KÖNYVET");
        result[0].MetaData.TipusId.Should().Be(Forras.ajanlatweb);
        result[0].ContentDetails.Cim.Should().Be("Cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.ajanlatweb);
        result.Count.Should().Be(1);
    }

    [Theory, AutoDataWithCustomData]
    public void GetAjanlat_Details_Happy(Content content)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        content.Tipus = (int)Forras.ajanlatweb;
        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetContentDetailsModelByNid(content.Nid, content.Tipus);

        result.MetaData.Image.Should().Be("images/gift.png");
        result.MetaData.TipusNev.Should().Be("AJÁNDéKOZZ KÖNYVET");
        result.MetaData.TipusId.Should().Be(Forras.ajanlatweb);
        result.ContentDetails.Cim.Should().Be("Cím");
        result.ContentDetails.Tipus.Should().Be((int)Forras.ajanlatweb);
    }

    [Theory, AutoDataWithCustomData]
    public void GetAjanlat_Details_No_Result(Content content)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        content.Tipus = (int)Forras.papaitwitter;
        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());
        var result = contentService.GetContentDetailsModelByNid(Guid.NewGuid(), (int)Forras.ajanlatweb);

        result.Should().BeNull();
    }

    [Theory, AutoDataWithCustomData]
    public void GetSzentek_Happy(FixContent fixContent)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        fixContent.Tipus = (int)Forras.pio;
        context.FixContent.Add(fixContent);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetSzentek().ToList();

        result[0].MetaData.Image.Should().Be("images/pio.png");
        result[0].MetaData.TipusNev.Should().Be("Pio atya breviáriuma");
        result[0].MetaData.TipusId.Should().Be(Forras.pio);
        result[0].ContentDetails.Cim.Should().Be("Cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.pio);
        result.Count.Should().Be(1);
    }

    [Theory, AutoDataWithCustomData]
    public void GetImaPage_Happy_No_Csoport(ImaContent ima)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        context.ImaContent.Add(ima);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetImaPage(string.Empty).ToList();

        result[0].ContentDetails.Cim.Should().Be("Cím");
        result.Count.Should().Be(1);
    }

    [Theory, AutoDataWithCustomData]
    public void GetImaPage_Happy_Has_Csoport(ImaContent ima, ImaContent ima2)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        ima.Cim = "Ima cím1";
        context.ImaContent.Add(ima);
        ima2.Nid = Guid.NewGuid();
        ima2.Cim = "Ima cím2";
        ima2.Csoport = "2";
        context.ImaContent.Add(ima2);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetImaPage("1").ToList();

        result[0].ContentDetails.Cim.Should().Be("Ima cím1");
        result.Count.Should().Be(1);
    }

    [Theory, AutoDataWithCustomData]
    public void GetImaDetailsPage_Happy(ImaContent ima)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        ima.Szoveg = "Szöveg";
        context.ImaContent.Add(ima);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetImaById(ima.Nid);

        result.Cim.Should().Be("Cím");
        result.Csoport.Should().Be("1");
        result.Szoveg.Should().Be("Szöveg");
    }

    [Theory, AutoData]
    public void GetAudio_Happy(Content content, Content content2)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        content.Cim = "cim1";
        content.Nid = Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35");
        content.Tipus = (int)Forras.audiotaize;
        content.Fulldatum = now.DateTime.ToString("yyyy") + "-02-03";
        content.Idezet = "audiofile";
        content.FileUrl = "";

        context.Content.Add(content);

        content2.Cim = "cim2";
        content2.Nid = Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35");
        content2.Tipus = (int)Forras.audiotaize;
        content2.Fulldatum = now.AddDays(-1).DateTime.ToString("yyyy") + "-02-03";
        content2.Idezet = "idezet"; //ezt felül kell írja a FileUrl audio típusnál
        content2.FileUrl = "audiofile";

        context.Content.Add(content2);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var service = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var audioRecords = service.GetAudio().ToList();

        string expected = "<div></div><div style=\"margin: 15px 0 0 0;\"><strong>Hangállomány meghallgatása</strong></div><div style=\"margin: 0 0 15px 0;\"><audio controls><source src=\"audiofile\" type=\"audio/mpeg\">Your browser does not support the audio element.</audio></div><div style=\"margin: 0 0 15px 0;word-break: break-all;\"><strong>Vagy a letöltése</strong>: <a href=\"audiofile\">audiofile</a></div>";

        audioRecords[0].Idezet.Should().Be(expected);
        audioRecords[1].Idezet.Should().Be(expected);
    }

    [Theory, AutoData]
    public void GetVideoSource_Happy(VideoContent video)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        video.Cim = "Videó cím";
        video.Nid = guid;
        video.ChannelId = "UCF3mEbdkhZwjQE8reJHm4sg";
        video.Forras = "Forras";
        video.VideoLink = "https://www.youtube.com/embed/OnCW6hg5CdQ";

        context.VideoContent.Add(video);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var videoPage = contentService.GetVideoSourcePage().ToList();

        videoPage[0].ChannelId.Should().Be("UCF3mEbdkhZwjQE8reJHm4sg");
        videoPage[0].ChannelName.Should().Be("Forras");
        videoPage.Count.Should().Be(1);
    }

    [Theory]
    [InlineData((int)Forras.b777)]
    [InlineData(0)]
    public void GetBlog_Happy(int tipus)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
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

        var result = contentService.GetBlog(tipus);
        foreach (var item in result)
        {
            item.ContentDetails.Cim.Should().Be("Blog cím");
        }

        result.Count.Should().Be(1);
    }

    [Theory]
    [InlineData((int)Forras.b777)]
    [InlineData(0)]
    public void GetBlog_NotFound(int tipus)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetBlog(tipus).ToList();

        result.Count.Should().Be(0);
    }

    [Theory]
    [InlineData((int)Forras.kurir)]
    [InlineData(0)]
    public void GetHirek_Happy(int tipus)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var content = new Content
        {
            Cim = "Hír cím",
            Nid = guid,
            Tipus = (int)Forras.kurir,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03",
            Inserted = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-02-03"),
            Idezet = "Idézet",
            KulsoLink = "KulsoLink"
        };

        context.Content.Add(content);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHirek(tipus);
        foreach (var item in result)
        {
            item.ContentDetails.Cim.Should().Be("Hír cím");
            item.ContentDetails.Idezet.Should().Be("Idézet");
            item.ContentDetails.KulsoLink.Should().Be("KulsoLink");
        }

        result.Count.Should().Be(1);
    }

    [Theory]
    [InlineData((int)Forras.kurir)]
    [InlineData(0)]
    public void GetHirek_NotFound(int tipus)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHirek(tipus).ToList();

        result.Count.Should().Be(0);
    }

    [Theory, AutoDataWithCustomData]
    public void GetHumor_Happy(FixContent fixContent)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        fixContent.Tipus = (int)Forras.humor;
        context.FixContent.Add(fixContent);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetHumor().ToList();

        result[0].MetaData.Image.Should().Be("images/smile.jpg");
        result[0].MetaData.TipusNev.Should().Be("Humor");
        result[0].MetaData.TipusId.Should().Be(Forras.humor);
        result[0].ContentDetails.Cim.Should().Be("Cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.humor);
        result.Count.Should().Be(1);
    }

    [Theory, AutoData]
    public void GetRadioPage_Happy(SystemInfo systeminfo)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        systeminfo.Key = "radio";
        systeminfo.Value = "{ \"ˇmariaradio\": \"url\"}";

        context.SystemInfo.Add(systeminfo);
        context.SaveChanges();

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetRadioPage().ToList();

        result[0].Name.Should().Be("ˇmariaradio");
        result[0].Url.Should().Be("url");
        result.Count.Should().Be(1);
    }

    [Fact]
    public void GetRadioPage_NotFound()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var repository = new CacheRepository(context, loggerRepo.Object, GetClock().Object, mapper);
        var cacheService = new CacheService(repository, new Mock<ILogger<CacheService>>().Object, GetClock().Object);
        var contentService = new ContentService(GetIMemoryCache(), cacheService, GetAndrokatConfiguration());

        var result = contentService.GetRadioPage().ToList();

        result.Count.Should().Be(0);
    }

    [Theory, AutoData]
    public void GetSzent_Happy(Maiszent maiszent)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<CacheRepository>>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
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