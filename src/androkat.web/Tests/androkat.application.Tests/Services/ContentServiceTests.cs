using androkat.application.Service;
using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using FluentAssertions;
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

        var service = new ContentService(repository.Object);

        var result = service.GetHome().ToList();

        result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
        result[0].MetaData.TipusNev.Should().Be("Ferenc pápa twitter üzenete");
        result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[0].ContentDetails.Cim.Should().Be("Twitter cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
        result.Count.Should().Be(2);
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

        var service = new ContentService(repository.Object);

        var result = service.GetAjanlat().ToList();

        result[0].MetaData.Image.Should().Be("images/Ajánlat.png");
        result[0].MetaData.TipusNev.Should().Be("Ajánlat");
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

        var service = new ContentService(repository.Object);

        var result = service.GetSzentek().ToList();

        result[0].MetaData.Image.Should().Be("images/pio.png");
        result[0].MetaData.TipusNev.Should().Be("Szentek");
        result[0].MetaData.TipusId.Should().Be(Forras.pio);
        result[0].ContentDetails.Cim.Should().Be("Pio cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.pio);
        result.Count.Should().Be(1);
    }
}