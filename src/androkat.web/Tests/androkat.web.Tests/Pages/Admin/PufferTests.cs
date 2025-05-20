using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using androkat.web.Pages.Ad;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace androkat.web.Tests.Pages.Admin;

public class PufferTests : BaseTest
{
	[Fact]
	public void OnGet_With_Data_Test()
	{
		var _fixture = new Fixture();

		var (pageContext, tempData, actionContext) = GetPreStuff();
		var logger = new Mock<ILogger<PufferModel>>();

		var loggerForras = new Mock<ILogger<ContentMetaDataService>>();
		var idezetData = Options.Create(new AndrokatConfiguration
		{
			ContentMetaDataList = new ContentMetaDataService(loggerForras.Object).GetContentMetaDataList()
		});

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

		var loggerRepo = new Mock<ILogger<AdminRepository>>();

		using var context = new AndrokatContext(GetDbContextOptions());

		var guid = Guid.NewGuid();
		var tempContent = _fixture.Create<TempContent>();
		tempContent.Tipus = (int)Forras.audiohorvath;
		tempContent.Cim = "audiohorvathCim";
		tempContent.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-01";
		tempContent.Nid = guid;

		context.TempContent.Add(tempContent);
		context.SaveChanges();

		var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);

		var model = new PufferModel(logger.Object, clock.Object, repo, idezetData)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext)
		};

		model.OnGet(6, guid.ToString(), null);
		model.Cim.Should().Be("audiohorvathCim");
		model.Nid.Should().Be(guid.ToString());
		model.TipusNev.Should().Be("Horváth István Sándor (evangélium365.hu)");

		model.TipusId.Should().Be(6);
	}

	[Fact]
	public void OnGet_Failed_Auth_Test()
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();

		var _logger = new Mock<ILogger<IndexModel>>();
		var adminRepository = new Mock<IAdminRepository>();
		var authService = new Mock<IAuthService>();
		authService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(false);

		var generalConfig = Options.Create(new GeneralConfiguration());

		var model = new IndexModel(_logger.Object, adminRepository.Object, authService.Object, generalConfig)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext)
		};

		dynamic res = model.OnGet();
		string url = res.Url;
		url.Should().Be("/");
	}

	private static (PageContext pageContext, TempDataDictionary tempData, ActionContext actionContext) GetPreStuff()
	{
		var httpContext = new DefaultHttpContext();
		var modelState = new ModelStateDictionary();
		var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
		var modelMetadataProvider = new EmptyModelMetadataProvider();
		var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
		var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
		var pageContext = new PageContext(actionContext)
		{
			ViewData = viewData
		};

		return (pageContext, tempData, actionContext);
	}
}