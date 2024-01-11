using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class PartnerRepositoryTests : BaseTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
    }
}
