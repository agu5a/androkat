using androkat.application.Interfaces;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class VideoModelTests : BaseTest
{
    [TestCase("ChannelId", "ChannelId")]
    [TestCase("", "")]
    [TestCase(null, "")]
    public void VideoModelTest(string actual, string expected)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetVideoSourcePage()).Returns(new List<VideoSourceModel>
		{
            new("ChannelId", "ChannelName")
        });

        var model = new web.Pages.VideoModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            F = actual
        };

        model.OnGet();
        model.VideoSourceModels.First().ChannelName.Should().Be("ChannelName");
        model.ViewData["source"].ToString().Should().Be(expected);
    }
}