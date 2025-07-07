using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class ApiRepositoryTests : BaseTest
{
    [Fact]
    public void GetSystemInfoModels_Exist()
    {
        var logger = new Mock<ILogger<ApiRepository>>();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();
        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        context.SystemInfo.Add(new SystemInfo { Id = 0, Key = "key", Value = "value" });
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);

        var result = repo.GetSystemInfoModels();
        result.First().Key.Should().Be("key");
        result.Count().Should().Be(1);
    }

    [Fact]
    public void AddVideo_Exist()
    {
        Guid nid = Guid.NewGuid();
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        context.VideoContent.Add(new VideoContent { Nid = nid, VideoLink = "videoLink" });
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);

        var model = new VideoModel(nid, "kép", "videoLink", "cím", "2023-01-10", "forras", "channelId", clock.Object.Now.DateTime);

        var result = repo.AddVideo(model);
        result.Should().Be(false);
        context.VideoContent.Count().Should().Be(1);
    }

    [Fact]
    public void AddVideo_NotFound()
    {
        Guid nid = Guid.NewGuid();
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new ApiRepository(context, clock.Object, mapper);

        var model = new VideoModel(nid, "kép", "videoLink", "cím", "2023-01-10", "forras", "channelId", clock.Object.Now.DateTime);

        var result = repo.AddVideo(model);
        result.Should().Be(true);
        context.VideoContent.Count().Should().Be(1);
    }

    [Fact]
    public void DeleteVideoByNid_NotFound()
    {
        Guid nid = Guid.NewGuid();
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        context.VideoContent.Add(new VideoContent { Nid = Guid.NewGuid() });
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.DeleteVideoByNid(nid);
        result.Should().Be(false);
        context.VideoContent.Count().Should().Be(1);
    }

    [Fact]
    public void UpdateRadioSystemInfo_Exist()
    {
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        context.SystemInfo.Add(new SystemInfo { Id = 0, Key = "radio", Value = "value" });
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.UpdateRadioSystemInfo("newvalue");
        result.Should().Be(true);
        context.SystemInfo.FirstOrDefault(f => f.Key == "radio").Value.Should().Be("newvalue");
    }

    [Fact]
    public void UpdateRadioSystemInfo_NotFound()
    {
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.UpdateRadioSystemInfo("newvalue");
        result.Should().Be(false);
    }

    [Fact]
    public void UpdateRadioMusor_NotFound()
    {
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.UpdateRadioMusor(new domain.Model.RadioMusorModel(Guid.Empty, "Source", string.Empty, string.Empty));
        result.Should().Be(false);
        context.RadioMusor.FirstOrDefault(f => f.Source == "Source").Should().BeNull();
    }

    [Fact]
    public void UpdateRadioMusor_Happy()
    {
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        Guid nid = Guid.NewGuid();
        var entity = new RadioMusor
        {
            Source = "Source",
            Inserted = "2023-01-10",
            Musor = "Műsor",
            Nid = nid
        };
        context.RadioMusor.Add(entity);
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.UpdateRadioMusor(new domain.Model.RadioMusorModel(Guid.Empty, "Source", "Műsor 2", "2023-01-11"));
        result.Should().Be(true);
        var radio = context.RadioMusor.FirstOrDefault(f => f.Source == "Source");
        radio.Musor.Should().Be("Műsor 2");
        radio.Inserted.Should().Be("2023-01-11");
        radio.Nid.Should().Be(nid);
    }

    [Theory]
    [InlineData(1, "cím1", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B", 1, "cím1", "AA4E35F9-0875-49E9-8A19-67AD429BE747")]
    [InlineData(1, "cím1", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B", 1, "cím2", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B")]
    public void AddContentDetailsModel_Conflict(int tipusDb, string cimDb, string guidDb, int tipus, string cim, string guid)
    {
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        var nidDb = Guid.Parse(guidDb);
        var nid = Guid.Parse(guid);
        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = tipusDb,
            Cim = cimDb,
            Nid = nidDb
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.AddContentDetailsModel(new domain.Model.ContentDetailsModel(nid, DateTime.MinValue, cim, string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        );
        result.Should().Be(false);
        context.Content.FirstOrDefault(f => f.Nid == nid && f.Cim == cim).Should().BeNull();
    }

    [Theory]
    [InlineData(1, "cím1", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B", 1, "cím2", "AA4E35F9-0875-49E9-8A19-67AD429BE747")]
    public void AddContentDetailsModel_Happy(int tipusDb, string cimDb, string guidDb, int tipus, string cim, string guid)
    {
        var logger = new Mock<ILogger<ApiRepository>>();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = GetToday();

        var nidDb = Guid.Parse(guidDb);
        var nid = Guid.Parse(guid);
        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = tipusDb,
            Cim = cimDb,
            Nid = nidDb
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.AddContentDetailsModel(new domain.Model.ContentDetailsModel(nid, DateTime.MinValue, cim, string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        );
        result.Should().Be(true);
        context.Content.FirstOrDefault(f => f.Nid == nid && f.Cim == cim).Should().NotBeNull();
    }
}
