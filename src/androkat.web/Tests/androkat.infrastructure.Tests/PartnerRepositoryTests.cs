using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using MockQueryable;

namespace androkat.infrastructure.Tests;

public class PartnerRepositoryTests : BaseTest
{
    [Fact]
    public void GetTempContentByNid_Returns_Null()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new PartnerRepository(context, logger.Object, clock.Object, mapper);
        var result = repo.GetTempContentByNid(Guid.NewGuid().ToString());
        result.Should().BeNull();
    }

    [Fact]
    public void GetTempContentByTipus_Returns_Empty()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new PartnerRepository(context, logger.Object, clock.Object, mapper);
        var result = repo.GetTempContentByTipus(6);
        result.Should().BeEmpty();
    }

    [Fact]
    public void InsertTempContent_Returns_True()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new PartnerRepository(context, logger.Object, clock.Object, mapper);
        var result = repo.InsertTempContent(new ContentDetailsModel(Guid.NewGuid(), DateTime.Now, "cim", "idezet", 6, DateTime.Now));
        result.Should().BeTrue();

        context.TempContent.FirstOrDefault(f => f.Tipus == 6).Should().NotBeNull();
    }

    [Fact]
    public void InsertTempContent_Returns_False()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();
        Guid guid = Guid.NewGuid();

        var mock = new List<TempContent>().BuildMock().BuildMockDbSet();
        mock.Setup(x => x.Find(guid)).Throws(new Exception());

        var contextMock = new Mock<AndrokatContext>();
        contextMock.Setup(x => x.TempContent).Returns(mock.Object);

        var repo = new PartnerRepository(contextMock.Object, logger.Object, clock.Object, mapper);
        var result = repo.InsertTempContent(new ContentDetailsModel(guid, DateTime.Now, "cim", "idezet", 6, DateTime.Now));
        result.Should().BeFalse();
    }

    [Fact]
    public void DeleteTempContentByNid_Returns_True()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new PartnerRepository(context, logger.Object, clock.Object, mapper);
        Guid nid = Guid.NewGuid();
        var result = repo.InsertTempContent(new ContentDetailsModel(nid, DateTime.Now, "cim", "idezet", 6, DateTime.Now));
        result.Should().BeTrue();
        context.TempContent.FirstOrDefault(f => f.Tipus == 6).Should().NotBeNull();

        result = repo.DeleteTempContentByNid(nid.ToString());
        result.Should().BeTrue();
        context.TempContent.FirstOrDefault(f => f.Tipus == 6).Should().BeNull();
    }

    [Fact]
    public void DeleteTempContentByNid_NotExists()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();
        Guid guid = Guid.NewGuid();

        var mock = new List<TempContent>().BuildMock().BuildMockDbSet();
        mock.Setup(x => x.Find(guid)).Throws(new Exception());

        var contextMock = new Mock<AndrokatContext>();
        contextMock.Setup(x => x.TempContent).Returns(mock.Object);        

        var repo = new PartnerRepository(contextMock.Object, logger.Object, clock.Object, mapper);
        
        var result = repo.DeleteTempContentByNid(guid.ToString());
        result.Should().BeFalse();
    }

    [Fact]
    public void LogInUser_Returns_False()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new PartnerRepository(context, logger.Object, clock.Object, mapper);
        var result = repo.LogInUser("aa@aa.hu");
        result.Should().BeFalse();

        context.Admin.FirstOrDefault(f => f.Email == "aa@aa.hu").Should().NotBeNull();
        context.Admin.FirstOrDefault().LastLogin.ToString("yyyy-MM-ddTHH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06");
    }

    [Fact]
    public void LogInUser_Returns_True()
    {
        var logger = new Mock<ILogger<PartnerRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        context.Admin.Add(new Admin { Email = "aa@aa.hu", Id = 1, LastLogin = GetToday().Object.Now.AddHours(-1).DateTime });
        context.SaveChanges();

        var repo = new PartnerRepository(context, logger.Object, clock.Object, mapper);
        var result = repo.LogInUser("aa@aa.hu");
        result.Should().BeTrue();

        context.Admin.FirstOrDefault().LastLogin.ToString("yyyy-MM-ddTHH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06");
    }
}
