using androkat.businesslayer.Mapper;
using androkat.businesslayer.Service;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;

namespace androkat.businesslayer.Tests.Services;

public class MainPageServiceTests
{
    [Test]
    public void GetHome_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var service = new MainPageService(mapper);

        var result = service.GetHome();

        result[0].IdezetData.Image.Should().Be("Image");
        result[0].Napiolvaso.Cim.Should().Be("Twitter cím");
    }
}
