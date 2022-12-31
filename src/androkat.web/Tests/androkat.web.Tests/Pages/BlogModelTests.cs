using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using NUnit.Framework;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class BlogModelTests : BaseTest
{
    [Test]
    public void BlogModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.BlogModel(GetContentService((int)Forras.b777).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Source = "b777"
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be((int)Forras.b777);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}