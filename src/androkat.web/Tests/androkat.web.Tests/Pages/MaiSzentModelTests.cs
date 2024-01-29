using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class MaiSzentModelTests : BaseTest
{
    [Fact]
    public void MaiSzentModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.SzentModel(GetContentService((int)Forras.maiszent).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Cim");
        model.ContentModels.First().ContentDetails.Img.Should().Be("Image");
        model.ContentModels.First().ContentDetails.Tipus.Should().Be((int)Forras.maiszent);
        model.ContentModels.First().MetaData.Image.Should().Be("Image");
    }
}