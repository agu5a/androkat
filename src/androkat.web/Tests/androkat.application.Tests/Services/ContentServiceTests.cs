using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class ContentServiceTests : BaseTest
{
    [Test]
    public void GetHome_Happy()
    {
        Mock<IContentRepository> repository = GetContentRepository(
            new List<ContentModel>
            {
                new ContentModel
                {
                    ContentDetails = new ContentDetailsModel
                    {
                        Cim = "Twitter cím",
                        Fulldatum = DateTime.Now,
                        Nid = Guid.NewGuid(),
                        Tipus = (int)Forras.papaitwitter
                    },
                    MetaData = new ContentMetaDataModel
                    {
                        TipusId = Forras.papaitwitter,
                        TipusNev = "Ferenc pápa twitter üzenete",
                        Image = "images/ferencpapa.png"
                    }
                },
                new ContentModel
                {
                    ContentDetails = new ContentDetailsModel
                    {
                        Cim = "Advent cím",
                        Fulldatum = DateTime.Now,
                        Nid = Guid.NewGuid(),
                        Tipus = (int)Forras.advent
                    },
                    MetaData = new ContentMetaDataModel
                    {
                        TipusId = Forras.advent,
                        TipusNev = "Advent",
                        Image = "images/advent.png"
                    }
                }
            });

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var logger = new Mock<ILogger<ContentMetaDataService>>();
        var service = new ContentMetaDataService(logger.Object);
        var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = metaDataList
        });

        var contentService = new ContentService(mapper, repository.Object, contentMetaDataModels);

        var result = contentService.GetHome().ToList();

        result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
        result[0].MetaData.TipusNev.Should().Be("Ferenc pápa twitter üzenete");
        result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[0].ContentDetails.Cim.Should().Be("Twitter cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
        result.Count.Should().Be(3);
    }

    [Test]
    public void GetAjanlat_Happy()
    {
        Mock<IContentRepository> repository = GetContentRepository(
            new List<ContentModel>
            {
                new ContentModel
                {
                    ContentDetails = new ContentDetailsModel
                    {
                        Cim = "Ajánlat cím",
                        Fulldatum = DateTime.Now,
                        Nid = Guid.NewGuid(),
                        Tipus = (int)Forras.ajanlatweb
                    },
                    MetaData = new ContentMetaDataModel
                    {
                        TipusId = Forras.ajanlatweb,
                        TipusNev = "Ajánlat",
                        Image = "images/Ajánlat.png"
                    }
                }
            });

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var logger = new Mock<ILogger<ContentMetaDataService>>();
        var service = new ContentMetaDataService(logger.Object);
        var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = metaDataList
        });

        var contentService = new ContentService(mapper, repository.Object, contentMetaDataModels);

        var result = contentService.GetAjanlat().ToList();

        result[0].MetaData.Image.Should().Be("images/gift.png");
        result[0].MetaData.TipusNev.Should().Be("AJÁNDéKOZZ KÖNYVET");
        result[0].MetaData.TipusId.Should().Be(Forras.ajanlatweb);
        result[0].ContentDetails.Cim.Should().Be("Ajánlat cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.ajanlatweb);
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetSzentek_Happy()
    {
        Mock<IContentRepository> repository = GetContentRepository(
            new List<ContentModel>
            {
                new ContentModel
                {
                    ContentDetails = new ContentDetailsModel
                    {
                        Cim = "Pio cím",
                        Fulldatum = DateTime.Now,
                        Nid = Guid.NewGuid(),
                        Tipus = (int)Forras.pio
                    },
                    MetaData = new ContentMetaDataModel
                    {
                        TipusId = Forras.pio,
                        TipusNev = "Szentek",
                        Image = "images/pio.png"
                    }
                }
            });

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var logger = new Mock<ILogger<ContentMetaDataService>>();
        var service = new ContentMetaDataService(logger.Object);
        var metaDataList = service.GetContentMetaDataList("../../../../../androkat.web/Data/IdezetData.json");

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = metaDataList
        });

        var contentService = new ContentService(mapper, repository.Object, contentMetaDataModels);

        var result = contentService.GetSzentek().ToList();

        result[0].MetaData.Image.Should().Be("images/pio.png");
        result[0].MetaData.TipusNev.Should().Be("Pio atya breviáriuma");
        result[0].MetaData.TipusId.Should().Be(Forras.pio);
        result[0].ContentDetails.Cim.Should().Be("Pio cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.pio);
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetImaPage_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });

        var contentService = new ContentService(mapper, new Mock<IContentRepository>().Object, contentMetaDataModels);

        var result = contentService.GetImaPage(string.Empty).ToList();

        result[0].ContentDetails.Cim.Should().Be("Ima Cím");
        result.Count.Should().Be(1);
    }

    [Test]
    public void GetAudio_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });

        var contentService = new ContentService(mapper, new Mock<IContentRepository>().Object, contentMetaDataModels);

        var result = contentService.GetAudio().ToList();

        result[0].Cim.Should().Be("Audio cím");
        result.Count.Should().Be(6);
    }

    [Test]
    public void GetVideoSource_Happy()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var contentMetaDataModels = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });

        var contentService = new ContentService(mapper, new Mock<IContentRepository>().Object, contentMetaDataModels);

        var result = contentService.GetVideoSourcePage().ToList();

        result[0].ChannelId.Should().Be("UCF3mEbdkhZwjQE8reJHm4sg");
        result.Count.Should().Be(1);
    }
}