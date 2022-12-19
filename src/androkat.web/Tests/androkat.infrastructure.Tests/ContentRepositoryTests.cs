using androkat.application.Service;
using androkat.domain.Enum;
using androkat.infrastructure.Configuration;
using androkat.infrastructure.DataManager.SQLite;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class ContentRepositoryTests
{
    [Test]
    public void GetContentDetailsModel_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var loggerRepo = new Mock<ILogger<ContentRepository>>();

        var logger = new Mock<ILogger<ContentMetaDataService>>();
        var service = new ContentMetaDataService(logger.Object);
        var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

        var idezetData = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = metaDataList
        });

        var repo = new ContentRepository(loggerRepo.Object, mapper, idezetData);

        var result = repo.GetContentDetailsModel(new List<int>()).ToList();

        result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
        result[0].MetaData.TipusNev.Should().Be("Ferenc pápa twitter üzenete");
        result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[0].ContentDetails.Cim.Should().Be("Twitter cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
        result.Count.Should().Be(3);
    }
}