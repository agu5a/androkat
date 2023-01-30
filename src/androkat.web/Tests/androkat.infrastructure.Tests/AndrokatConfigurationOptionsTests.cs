using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class AndrokatConfigurationOptionsTests : BaseTest
{
    [Test]
    public void Configure()
    {
        var logger = new Mock<ILogger<AndrokatConfigurationOptions>>();

        var service = new Mock<IContentMetaDataService>();
        service.Setup(s => s.GetContentMetaDataList(It.IsAny<string>())).Returns(new List<ContentMetaDataModel> { GetContentMetaDataModel(Forras.mello) });
        var options = new AndrokatConfigurationOptions(service.Object, logger.Object);
        var config = new AndrokatConfiguration();
        options.Configure(config);

        config.ContentMetaDataList.First().TipusId.Should().Be(Forras.mello);
        config.GetContentMetaDataModelByTipus(1).TipusId.Should().Be(Forras.mello);
    }
}