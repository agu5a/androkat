using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class HirekModelTests : BaseTest
{
    [TestCase("kurir", (int)Forras.kurir)]
    [TestCase("keresztenyelet", (int)Forras.keresztenyelet)]
    [TestCase("bonumtv", (int)Forras.bonumtv)]
    [TestCase("", (int)Forras.kurir)]
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