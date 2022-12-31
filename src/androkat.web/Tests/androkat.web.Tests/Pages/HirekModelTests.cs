using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using NUnit.Framework;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class HirekModelTests : BaseTest
{
    [Test]
    public void HirekModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.HirekModel(GetContentService((int)Forras.kurir).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Source = "kurir"
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be((int)Forras.kurir);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}