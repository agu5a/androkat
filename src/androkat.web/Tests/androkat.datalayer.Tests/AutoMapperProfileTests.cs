using androkat.datalayer.Mapper;
using androkat.datalayer.Model.SQLite;
using androkat.domain.Model;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;

namespace androkat.datalayer.Tests;

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
            Fulldatum = "2022-12-15 01:02:03"
        });

        result.Cim.Should().BeNull();
        result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
    }

    //[Test]
    //public void Map_Happy()
    //{
    //    var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
    //    var mapper = config.CreateMapper();

    //    var _fixture = new Fixture();
    //    var napifixolvaso = _fixture.Create<Napifixolvaso>();
    //    napifixolvaso.Datum = "02-05";
    //    var napiolvaso = mapper.Map<Napiolvaso>(napifixolvaso);
    //    napiolvaso.Cim.Should().Be(napifixolvaso.Cim);
    //    napiolvaso.Idezet.Should().Be(napifixolvaso.Idezet);
    //    napiolvaso.Nid.Should().Be(napifixolvaso.Nid);
    //    napiolvaso.Tipus.Should().Be(napifixolvaso.Tipus);
    //    napiolvaso.Fulldatum.Should().Be(napifixolvaso.Datum);
    //    napiolvaso.FileUrl.Should().Be(string.Empty);
    //}
}