using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using NUnit.Framework;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class BlogModelTests : BaseTest
{
    [TestCase("b777", (int)Forras.b777)]
    [TestCase("bzarandokma", (int)Forras.bzarandokma)]
    [TestCase("jezsuitablog", (int)Forras.jezsuitablog)]
    [TestCase("", (int)Forras.b777)]
    public void BlogModelTest(string source, int tipus)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.BlogModel(GetContentService(tipus).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Source = source
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be(tipus);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}