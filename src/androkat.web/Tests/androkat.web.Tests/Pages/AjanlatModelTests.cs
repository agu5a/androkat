using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class AjanlatModelTests : BaseTest
{
    [Fact]
    public void AjanlatModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.AjanlatModel(GetContentService((int)Forras.ajanlatweb).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be((int)Forras.ajanlatweb);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}