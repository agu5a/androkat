using androkat.application.Interfaces;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class IndexModelTests : BaseTest
{
    [Test]
    public void IndexModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var mainPageService = new Mock<IMainPageService>();
        mainPageService.Setup(s => s.GetHome()).Returns(new List<ContentModel>
        {
            new ContentModel
            {
                ContentDetails = new ContentDetailsModel{ Cim = "Cim", Tipus = 9 },
                MetaData = new ContentMetaDataModel{ Image = "Image" }
            }
        });

        var model = new web.Pages.IndexModel(mainPageService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.MainViewModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.MainViewModels.First().ContentDetails.Tipus.Should().Be(9);
        model.MainViewModels.First().MetaData.Image.Should().Be("Image");
    }
}