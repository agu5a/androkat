using androkat.businesslayer.Mapper;
using androkat.businesslayer.Model;
using androkat.datalayer.Model;
using androkat.datalayer.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;

namespace androkat.businesslayer.Tests;

public class AutoMapperProfileTests
{
    [Test]
    public void Map_IdezetData_IdezetDataViewModel()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<IdezetData, IdezetDataViewModel>(new IdezetData());

        result.Image.Should().BeNull();
    }

    [Test]
    public void Map_Napiolvaso_NapiOlvasoViewModel()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<Napiolvaso, NapiOlvasoViewModel>(new Napiolvaso
        {
            Datum = "2022-12-15 01:02:03"
        });

        result.Cim.Should().BeNull();
        result.Datum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
    }
}
