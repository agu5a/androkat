using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace androkat.infrastructure.Tests;

public class AutoMapperProfileTests
{
    [Test]
    public void Map_Napiolvaso_ContentDetailsModel()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<Napiolvaso, ContentDetailsModel>(new Napiolvaso
        {
            Fulldatum = "2022-12-15 01:02:03"
        });

        result.Cim.Should().BeNull();
        result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
    }

    [Test]
    public void Map_ContentDetailsModel_Napiolvaso()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<ContentDetailsModel, Napiolvaso>(new ContentDetailsModel
        {
            Fulldatum = DateTime.Parse("2022-12-15 01:02:03")
        });

        result.Cim.Should().BeNull();
        result.Fulldatum.Should().Be("2022-12-15 01:02:03");
    }

    [Test]
    public void Map_FixContent_ContentDetailsModel()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<FixContent, ContentDetailsModel>(new FixContent
        {
            Datum = "2022-12-15 01:02:03"
        });

        result.Cim.Should().BeNull();
        result.FileUrl.Should().BeEmpty();
        result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
    }

    [Test]
    public void Map_Maiszent_ContentDetailsModel()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<Maiszent, ContentDetailsModel>(new Maiszent
        {
            Datum = "2022-12-15"
        });

        result.Cim.Should().BeNull();
        result.FileUrl.Should().BeEmpty();
        result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 00:00:01");
    }

    //[Test]
    //public void Map_Happy()
    //{
    //    var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
    //    var mapper = config.CreateMapper();

    //    var _fixture = new Fixture();
    //    var fixContent = _fixture.Create<FixContent>();
    //    fixContent.Datum = "02-05";
    //    var napiolvaso = mapper.Map<Napiolvaso>(fixContent);
    //    napiolvaso.Cim.Should().Be(fixContent.Cim);
    //    napiolvaso.Idezet.Should().Be(fixContent.Idezet);
    //    napiolvaso.Nid.Should().Be(fixContent.Nid);
    //    napiolvaso.Tipus.Should().Be(fixContent.Tipus);
    //    napiolvaso.Fulldatum.Should().Be(fixContent.Datum);
    //    napiolvaso.FileUrl.Should().Be(string.Empty);
    //}
}