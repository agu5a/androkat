using androkat.application.Interfaces;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Web;

namespace androkat.web.Tests.Pages;

public class ImaDetailsModelTests : BaseTest
{
    [TestCase("0")]
    public void ImaDetailsModelTest(string csoport)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();
        var nid = Guid.NewGuid();
        var datum = DateTime.Now;

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetImaById(nid)).Returns(
            new ImaModel
            {
                Cim = "Ima Cim",
                Szoveg = "Szöveg",
                Csoport = csoport,
                Nid = nid,
                Datum = datum
            });

        var model = new web.Pages.Ima.DetailsModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Nid = nid.ToString()
        };

        model.OnGet();
        model.ImaModel.Cim.Should().Be("Ima Cim");
        model.ImaModel.Csoport.Should().Be(csoport);
        model.ImaModel.Szoveg.Should().Be("Szöveg");
        model.ImaModel.Datum.Should().Be(datum);

        model.ShareUrl.Should().Be($"https://androkat.hu/ima/details/{nid}");
        model.EncodedUrl.Should().Be(HttpUtility.UrlEncode($"https://androkat.hu/ima/details/{nid}"));
        model.ShareTitle.Should().Be(HttpUtility.UrlEncode("Ima Cim"));
    }

    [Test]
    public void ImaDetailsModelTest_No_Result()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();
        var nid = Guid.NewGuid();

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetImaById(nid)).Returns(default(ImaModel));

        var model = new web.Pages.Ima.DetailsModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Nid = nid.ToString()
        };

        var result = model.OnGet();
        Assert.That(result, Is.TypeOf(typeof(RedirectResult)));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("blabla")]
    public void ImaDetailsModelTest_No_Valid_Guid(string nid)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.Ima.DetailsModel(new Mock<IContentService>().Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Nid = nid
        };

        var result = model.OnGet();
        Assert.That(result, Is.TypeOf(typeof(RedirectResult)));
    }
}