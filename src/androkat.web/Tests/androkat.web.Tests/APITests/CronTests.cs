using androkat.application.Interfaces;
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
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using androkat.web.Controllers;
using Microsoft.AspNetCore.Hosting;

namespace androkat.web.Tests.APITests;

public class CronTests : BaseTest
{
    [Theory]
    [InlineData((int)Forras.keresztenyelet, 1)]
    [InlineData(-1, 0)]
    public void GetKeresztenyelet(int tipus, int count)
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Cim = "cim";
        content.Tipus = tipus;
        content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";
        content.Inserted = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-02-03");

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var controller = new Cron(loggerRepo.Object, repository, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object);
        var resV1 = controller.GetKeresztenyelet();
        dynamic s2 = resV1.Result;
        List<string> result = s2.Value;
        result.Count.Should().Be(count);
        if (result.Count == 1)
        {
            result.First().Should().Be("cim");
        }
    }

    [Theory]
    [InlineData("2022-01-01", "2022-01-01", true)]
    [InlineData("2022-01-01", "2021-12-31", false)]
    public void HasNapiolvasoByDate(string date, string dbDate, bool expectedResult)
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Fulldatum = dbDate;
        content.Tipus = 2;
        content.Fulldatum = dbDate.ToString();

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var controller = new Cron(loggerRepo.Object, repository, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object);
        var resV1 = controller.HasNapiolvasoByDate(2, date);
        dynamic s2 = resV1.Result;
        bool result = s2.Value;
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void GetSzentbernat()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Fulldatum = "2022-12-06 00:00:01";
        content.Tipus = 50;

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var controller = new Cron(loggerRepo.Object, repository, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object);
        var resV1 = controller.GetSzentbernat();
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-06 00:00:01");
    }

    [Fact]
    public void GetSzentbernat_Nincs()
    {
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var controller = new Cron(loggerRepo.Object, new Mock<IApiRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object);//todo
        var resV1 = controller.GetSzentbernat();
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("0001-01-01 00:00:00");
    }

    [Fact]
    public void GetLastContentByTipus()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Fulldatum = "2022-12-06 00:00:01";
        content.Tipus = (int)Forras.kurir;
        content.Inserted = DateTime.Parse("2022-12-06 00:00:01");

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var controller = new Cron(loggerRepo.Object, repository, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object);
        var resV1 = controller.GetLastContentByTipus((int)Forras.kurir);
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-06 00:00:01");
    }

    [Fact]
    public void GetLastContentByTipus_Nincs()
    {
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var controller = new Cron(loggerRepo.Object, new Mock<IApiRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object);//todo
        var resV1 = controller.GetLastContentByTipus((int)Forras.kurir);
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("0001-01-01 00:00:00");
    }
}
