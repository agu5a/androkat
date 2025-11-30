using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class SzentModelTests : BaseTest
{
    [Fact]
    public void SzentModel_OnGet_ReturnsContentModels()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.SzentModel(GetContentService((int)Forras.maiszent).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.ContentModels.Should().NotBeNull();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}
