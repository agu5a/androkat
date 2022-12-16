using androkat.businesslayer.Service;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.core.Tests.Pages;

public class IndexModelTests : BaseTest
{
    [Test]
    public void IndexModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var mainPageService = new Mock<IMainPageService>();
        mainPageService.Setup(s => s.GetHome()).Returns(new List<MainViewModel>
    {
        new MainViewModel
        {
            Napiolvaso = new NapiOlvasoViewModel{ Cim = "Cim" },
            IdezetData = new IdezetDataViewModel{ Image = "Image" }
        }
    });

        var model = new androkat.core.Pages.IndexModel(mainPageService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.MainViewModels.First().Napiolvaso.Cim.Should().Be("Cim");
        model.MainViewModels.First().IdezetData.Image.Should().Be("Image");
    }
}
