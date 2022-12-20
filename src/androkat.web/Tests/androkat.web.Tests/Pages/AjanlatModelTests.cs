using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using NUnit.Framework;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class AjanlatModelTests : BaseTest
{
    [Test]
    public void AjanlatModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.AjanlatModel(GetMainPageService(58).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be(58);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}