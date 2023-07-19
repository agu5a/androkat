using androkat.application.Interfaces;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class RadioModelTests : BaseTest
{
    [TestCase("ChannelId", "ChannelId")]
    [TestCase("", "")]
    [TestCase(null, "")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public void RadioModelTest(string actual, string expected)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetRadioPage()).Returns(new List<RadioModel>
        {
			new RadioModel("name", "")
        });

        var model = new web.Pages.RadioModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        model.RadioModels.First().Name.Should().Be("name");
    }
}