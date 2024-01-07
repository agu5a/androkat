using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace androkat.application.Tests.Services;

public class CronServiceTests : BaseTest
{
    [Fact]
    public void DeleteOldRowsTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-03-13T04:05:06"));

        var logger = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        for (int i = 0; i < 4; i++)
        {
            var entity = new Content
            {
                Tipus = (int)Forras.audiohorvath,
                FileUrl = string.Empty,
                Fulldatum = DateTime.Now.ToString("yyyy") + "-02-1" + i
            };
            context.Content.Add(entity);
            context.SaveChanges();
        }

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, logger.Object, GetIMemoryCache());
        service.Start();

        (context.Content.Count()).Should().Be(3);
        var dates = context.Content.Select(s => s.Fulldatum).ToList();
        (dates.Contains("02-10")).Should().BeFalse();
    }

    [Fact]
    public void DeleteOldRowsTest_Tavalyi_de_van_mar_idei()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-01-01T04:05:06"));

        var logger = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        for (int i = 0; i < 4; i++)
        {
            var entity = new Content
            {
                Tipus = (int)Forras.audiohorvath,
                FileUrl = string.Empty,
                Fulldatum = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-1" + i //tavalyi
            };
            context.Content.Add(entity);
            context.SaveChanges();
        }

        var idei = new Content
        {
            Tipus = (int)Forras.audiohorvath,
            FileUrl = string.Empty,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-01-01" //idei
        };
        context.Content.Add(idei);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, logger.Object, GetIMemoryCache());
        service.Start();

        (context.Content.Count()).Should().Be(4);
        var dates = context.Content.Select(s => s.Fulldatum).ToList();
        (dates.Count == 4).Should().BeTrue();
        (dates.First() == DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-10").Should().BeTrue();
    }

    [Fact]
    public void DeleteOldRowsTest_Tavalyi_de_nincs_meg_idei()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-01-01T04:05:06"));

        var logger = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        for (int i = 0; i < 4; i++)
        {
            var entity = new Content
            {
                Tipus = (int)Forras.audiohorvath,
                FileUrl = string.Empty,
                Fulldatum = DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-1" + i //tavalyi
            };
            context.Content.Add(entity);
            context.SaveChanges();
        }

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, logger.Object, GetIMemoryCache());
        service.Start();

        (context.Content.Count()).Should().Be(3);
        var dates = context.Content.Select(s => s.Fulldatum).ToList();
        (dates.Count == 3).Should().BeTrue();
        (dates.First() == DateTime.Now.AddYears(-1).ToString("yyyy") + "-12-10").Should().BeTrue();
    }

    [Fact]
    public void DeleteOldVideoRowsTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var date = DateTimeOffset.Now;

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(date);

        var mock = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        for (int i = 0; i < 105; i++)
        {
            var entity = new VideoContent
            {
                Date = date.DateTime.AddMinutes(i * -1).ToString("yyyy-MM-dd HH:mm:ss")
            };
            context.VideoContent.Add(entity);
            context.SaveChanges();
        }

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, mock.Object, GetIMemoryCache());
        service.Start();

        (context.VideoContent.Count()).Should().Be(100);
    }

    [Fact]
    public void DeleteOldTest_NoDelete()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Now);

        var logger = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        for (int i = 0; i < 4; i++)
        {
            var entity = new Content
            {
                Tipus = (int)Forras.kurir,
                Fulldatum = DateTime.Now.AddDays(i * 1).ToString("yyyy-MM-dd HH:mm:ss")
            };
            context.Content.Add(entity);
            context.SaveChanges();
        }

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, logger.Object, GetIMemoryCache());
        service.Start();

        (context.Content.Count()).Should().Be(4);
        var dates = context.Content.Select(s => s.Fulldatum).ToList();
        dates.ForEach(f =>
        {
            (DateTime.Parse(f) >= DateTime.Now.AddDays(-1)).Should().BeTrue();
        });

        logger.VerifyLogging(LogLevel.Debug, Times.Never());
    }

    [Fact]
    public void DeleteOldTest_NoDelete_LessAsMinimum()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Now);

        var logger = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = (int)Forras.jezsuitablog,
            Fulldatum = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss")
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, logger.Object, GetIMemoryCache());
        service.Start();

        (context.Content.Count()).Should().Be(1);
        var dates = context.Content.Select(s => s.Fulldatum).ToList();
        dates.ForEach(f =>
        {
            (DateTime.Parse(f) >= DateTime.Now.AddDays(-6)).Should().BeTrue();
        });

        logger.VerifyLogging(LogLevel.Debug, Times.Never());
    }

    [Fact]
    public void DeleteOldTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var now = DateTimeOffset.Parse("2022-02-03 05:02:02");
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(now);

        var logger = new Mock<ILogger<CronService>>();

        using var context = new AndrokatContext(GetDbContextOptions());
        for (int i = 0; i < 4; i++)
        {
            var entity = new Content
            {
                Tipus = (int)Forras.kurir,
                Fulldatum = now.DateTime.AddDays(i * -1).ToString("yyyy-MM-dd HH:mm:ss"),
                Inserted = now.DateTime.AddDays(i * -1)
            };
            context.Content.Add(entity);
            context.SaveChanges();
        }

        var cache = GetIMemoryCache();
        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var service = new CronService(repository, clock.Object, logger.Object, GetIMemoryCache());
        service.Start();

        (context.Content.Count()).Should().Be(2);
        var dates = context.Content.Select(s => s.Fulldatum).ToList();
        dates.ForEach(f =>
        {
            (DateTime.Parse(f) > now.DateTime.AddDays(-2)).Should().BeTrue();
        });

        logger.VerifyLogging("delete from Content. 18 2", LogLevel.Debug, Times.Once());
    }
}