using androkat.application.Service;
using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace androkat.application.Tests.Services;

public class ContentMetaDataServiceTests
{
    [Test]
    public void GetContentMetaDataList()
    {
        var logger = new Mock<ILogger<ContentMetaDataService>>();
        var service = new ContentMetaDataService(logger.Object);
        var result = service.GetContentMetaDataList("../../../../../androkat.web/bin/Debug/net6.0/Data/IdezetData.json");

        result.Where(w => w.TipusId == Forras.pio).First().TipusNev.Should().Be("Pio atya breviáriuma");
    }
}