using androkat.application.Interfaces;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class ImaModelTests : BaseTest
{
    [Test]
    public void ImaModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetImaPage(string.Empty)).Returns(new List<ContentModel>
        {
            new ContentModel { ContentDetails = new ContentDetailsModel{ Cim = "Ima Cim" } }
        });

        var model = new web.Pages.Ima.IndexModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Csoport = string.Empty
        };

        model.OnGet();
        model.ContentModels.First().ContentDetails.Cim.Should().Be("Ima Cim");
    }
}