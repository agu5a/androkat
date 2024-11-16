using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Xunit;
using System;
using System.Web;

namespace androkat.web.Tests.Pages;

public class DetailsModelTests : BaseTest
{
    [Theory]
    [InlineData((int)Forras.maiszent)]
	[InlineData((int)Forras.ajanlatweb)]
	public void DetailsModelTest(int tipus)
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();
		var nid = Guid.NewGuid();

		var model = new web.Pages.DetailsModel(GetContentService(tipus).Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext),
			Nid = nid.ToString(),
			Type = tipus
		};

		model.OnGet();
		model.ContentModel.ContentDetails.Cim.Should().Be("Cim");
		model.ContentModel.ContentDetails.Img.Should().Be("Image");
		model.ContentModel.ContentDetails.Tipus.Should().Be(tipus);
		model.ContentModel.MetaData.Image.Should().Be("Image");

		model.ShareUrl.Should().Be($"https://androkat.hu/details/{nid}/{tipus}");
		model.EncodedUrl.Should().Be(HttpUtility.UrlEncode($"https://androkat.hu/details/{nid}/{tipus}"));
		model.ShareTitle.Should().Be(HttpUtility.UrlEncode("Cim"));
        model.Image.Should().Be(tipus == (int)Forras.ajanlatweb
            ? "https://androkat.hu/images/ajanlatok/Image"
            : "https://androkat.hu/images/szentek/Image");

        model.ForrasLink.Should().Be("MetaLink");
		model.ForrasText.Should().Be("MetaForras");
	}

    [Theory]
    [InlineData((int)Forras.szentbernat)]
	public void DetailsModelTest_NoImg(int tipus)
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();
		var nid = Guid.NewGuid();

		var model = new web.Pages.DetailsModel(GetContentService(tipus).Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext),
			Nid = nid.ToString(),
			Type = tipus
		};

		model.OnGet();
		model.ContentModel.ContentDetails.Cim.Should().Be("Cim");
		model.ContentModel.ContentDetails.Img.Should().Be("");
		model.ContentModel.ContentDetails.Tipus.Should().Be(tipus);
		model.ContentModel.MetaData.Image.Should().Be("Image");

		model.ShareUrl.Should().Be($"https://androkat.hu/details/{nid}/{tipus}");
		model.EncodedUrl.Should().Be(HttpUtility.UrlEncode($"https://androkat.hu/details/{nid}/{tipus}"));
		model.ShareTitle.Should().Be(HttpUtility.UrlEncode("Cim"));
		model.Image.Should().BeNull();		
		model.ForrasLink.Should().Be("Forras");
		model.ForrasText.Should().Be("Forras");
	}

	[Theory]
    [InlineData((int)Forras.szentbernat)]
    public void DetailsModelTest_No_Result(int tipus)
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();
        var nid = Guid.NewGuid();

        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetContentDetailsModelByNid(It.IsAny<Guid>(), (int)Forras.szentbernat)).Returns(default(ContentModel));

        var model = new web.Pages.DetailsModel(contentService.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Nid = nid.ToString(),
            Type = tipus
        };

        var result = model.OnGet();
        result.Should().BeOfType(typeof(RedirectResult));
    }

	[Theory]
	[InlineData((int)Forras.szentbernat)]
	public void DetailsModelTest_No_MetaData_Result(int tipus)
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();
		var nid = Guid.NewGuid();

		var contentService = new Mock<IContentService>();
		contentService.Setup(s => s.GetContentDetailsModelByNid(It.IsAny<Guid>(), (int)Forras.szentbernat))
            .Returns(new ContentModel 
            (
                new ContentDetailsModel(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty), default
            ));

		var model = new web.Pages.DetailsModel(contentService.Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext),
			Nid = nid.ToString(),
			Type = tipus
		};

		var result = model.OnGet();
		result.Should().BeOfType(typeof(RedirectResult));
	}

	[Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("blabla")]
    public void DetailsModelTest_No_Valid_Guid(string nid)
    {
		var tipus = (int)Forras.szentbernat;

        var (pageContext, tempData, actionContext) = GetPreStuff();

        var model = new web.Pages.DetailsModel(GetContentService(tipus).Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            Nid = nid,
            Type = tipus
        };

        var result = model.OnGet();
		result.Should().BeOfType(typeof(RedirectResult));        
    }
}