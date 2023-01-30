using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.web.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class HanganyagModelTests : BaseTest
{
    [Test]
    public void HanganyagModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var now = DateTime.Now;

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetAudio()).Returns(new List<AudioModel>
        {
            new AudioModel 
            {
                Cim = "Audio Cim",
                Idezet = "Idézet",
                Tipus = (int)Forras.audionapievangelium,
                Inserted = now,
                MetaDataModel= GetContentMetaDataModel(image: "Image")
            }
        });

        var model = new HanganyagModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.AudioModels.First().Cim.Should().Be("Audio Cim");
        model.AudioModels.First().Idezet.Should().Be("Idézet");
        model.AudioModels.First().Inserted.Should().Be(now);
        model.AudioModels.First().Tipus.Should().Be((int)Forras.audionapievangelium);
        model.AudioModels.First().MetaDataModel.Image.Should().Be("Image");
    }    
}