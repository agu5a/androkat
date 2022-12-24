using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.web.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class HanganyagModelTests : BaseTest
{
    [Test]
    public void HanganyagModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetAudio()).Returns(new List<AudioViewModel>
        {
            new AudioViewModel 
            {
                Cim = "Audio Cim",
                Idezet = "Idézet",
                Tipus = (int)Forras.audionapievangelium,
                MetaDataModel= new ContentMetaDataModel
                {
                    Image = "Image"
                }
            }
        });

        var model = new web.Pages.HanganyagModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.AudioViewModels.First().Cim.Should().Be("Audio Cim");
        model.AudioViewModels.First().Idezet.Should().Be("Idézet");
        model.AudioViewModels.First().Tipus.Should().Be((int)Forras.audionapievangelium);
        model.AudioViewModels.First().MetaDataModel.Image.Should().Be("Image");
    }    
}