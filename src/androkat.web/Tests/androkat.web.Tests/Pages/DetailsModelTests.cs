using androkat.domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using NUnit.Framework;
using System;
using System.Web;

namespace androkat.web.Tests.Pages;

public class DetailsModelTests : BaseTest
{
	[TestCase((int)Forras.maiszent)]
	[TestCase((int)Forras.ajanlatweb)]
	public void DetailsModelTest(int tipus)
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();
		var nid = Guid.NewGuid();

		var model = new web.Pages.DetailsModel(GetContentService(tipus).Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext),
			Nid = nid,
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
		if (tipus == (int)Forras.ajanlatweb)
			model.Image.Should().Be("https://androkat.hu/images/ajanlatok/Image");
		else
			model.Image.Should().Be("https://androkat.hu/images/szentek/Image");
		model.ForrasLink.Should().Be("MetaLink");
		model.ForrasText.Should().Be("MetaForras");
	}

	[TestCase((int)Forras.szentbernat)]
	public void DetailsModelTest_NoImg(int tipus)
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();
		var nid = Guid.NewGuid();

		var model = new web.Pages.DetailsModel(GetContentService(tipus).Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext),
			Nid = nid,
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
}