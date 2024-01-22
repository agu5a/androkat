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
            new("http://aa.hu", "ShareTitle", "http://aa2.hu", "Idézet", now,
                "Audio Cim", (int)Forras.audionapievangelium, GetContentMetaDataModel(image: "Image", segedlink: "http://aa.hu"))
        });

        var model = new HanganyagModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        var audio = model.AudioModels.First();
        audio.Cim.Should().Be("Audio Cim");
        audio.Idezet.Should().Be("Idézet");
        audio.Inserted.Should().Be(now);
        audio.Tipus.Should().Be((int)Forras.audionapievangelium);
        audio.Url.Should().Be("http://aa2.hu");
        audio.ShareTitle.Should().Be("ShareTitle");
        audio.EncodedUrl.Should().Be("http://aa.hu");
        audio.MetaDataModel.Segedlink.Should().Be("http://aa.hu");
        audio.MetaDataModel.Image.Should().Be("Image");
    }
}