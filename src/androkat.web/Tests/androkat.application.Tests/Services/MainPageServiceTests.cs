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

public class MainPageServiceTests
{
    private readonly Mock<IContentRepository> _sqliteRepository = new();

    [Test]
    public void GetHome_Happy()
    {
        _sqliteRepository.Setup(s => s.GetContentDetailsModel(new List<int>())).Returns
            (
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

        var service = new MainPageService(_sqliteRepository.Object);

        var result = service.GetHome().ToList();

        result[0].MetaData.Image.Should().Be("images/ferencpapa.png");
        result[0].MetaData.TipusNev.Should().Be("Ferenc pápa twitter üzenete");
        result[0].MetaData.TipusId.Should().Be(Forras.papaitwitter);
        result[0].ContentDetails.Cim.Should().Be("Twitter cím");
        result[0].ContentDetails.Tipus.Should().Be((int)Forras.papaitwitter);
        result.Count.Should().Be(2);
    }
}
