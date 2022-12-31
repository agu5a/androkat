using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using NUnit.Framework;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class SzentekModelTests : BaseTest
{
    [Test]
    public void SzentekModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.SzentekModel(GetContentService((int)Forras.pio).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be((int)Forras.pio);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}