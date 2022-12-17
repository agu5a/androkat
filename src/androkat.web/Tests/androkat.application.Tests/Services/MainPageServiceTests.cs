using androkat.application.Service;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;

namespace androkat.application.Tests.Services;

public class MainPageServiceTests
{
    [Test]
    public void GetHome_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var service = new MainPageService(mapper);

        var result = service.GetHome();

        result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
        result[0].ContentDetails.Cim.Should().Be("Twitter cím");
        result.Count.Should().Be(2);
    }
}
