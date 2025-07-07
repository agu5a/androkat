using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace androkat.application.Tests.Services;

public class BookRadioSysCacheFillUpTests : BaseTest
{
    [Fact]
    public void BookRadioSysCacheFillUp_Happy_test()
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

        var radioNid = Guid.NewGuid();

        var cacheRepository = new Mock<ICacheRepository>();
        cacheRepository.Setup(s => s.GetBooksToCache()).Returns(new List<ContentDetailsModel>
        {
            new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        });
        cacheRepository.Setup(s => s.GetRadioToCache()).Returns(new List<RadioMusorModel>
        {
            new(radioNid, string.Empty, string.Empty, string.Empty)
        });
        cacheRepository.Setup(s => s.GetSystemInfoToCache()).Returns(new List<SystemInfoModel>
        {
            new(1, default, default)
        });

        var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
        var res = cacheService.BookRadioSysCacheFillUp();

        res.Books.Count.Should().Be(1);
        res.RadioMusor.Count.Should().Be(1);
        res.RadioMusor.First().Nid.Should().Be(radioNid);
        res.SystemData.Count.Should().Be(1);
        res.SystemData.First().Id.Should().Be(1);
        res.Inserted.ToString("yyyy-MM-dd").Should().Be("2012-01-03");
    }

    [Fact]
    public void BookRadioSysCacheFillUp_Throws_Exception()
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
        cacheRepository.Setup(s => s.GetBooksToCache()).Throws<Exception>();

        var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);

        Action act = () => cacheService.BookRadioSysCacheFillUp();
        act.Should().NotThrow<Exception>();
    }
}