using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class HirekModelTests : BaseTest
{
    [Theory]
    [InlineData("kurir", (int)Forras.kurir)]
    [InlineData("keresztenyelet", (int)Forras.keresztenyelet)]
    [InlineData("bonumtv", (int)Forras.bonumtv)]
    [InlineData("", (int)Forras.kurir)]
    public void HirekModelTest(string source, int tipus)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.HirekModel(GetContentService(tipus).Object)
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