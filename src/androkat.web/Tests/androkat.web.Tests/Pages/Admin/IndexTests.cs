using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoFixture;
using AutoMapper;
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
using IndexModel = androkat.web.Pages.Ad.IndexModel;

namespace androkat.web.Tests.Pages.Admin;

public class IndexTests : BaseTest
{
	[Fact]
	public void OnGet_With_Data_Test()
	{
		var _fixture = new Fixture();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var (pageContext, tempData, actionContext) = GetPreStuff();
		var logger = new Mock<ILogger<IndexModel>>();

		var authService = new Mock<IAuthService>();
		authService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(true);

		var loggerForras = new Mock<ILogger<ContentMetaDataService>>();
		var idezetData = Options.Create(new AndrokatConfiguration
		{
			ContentMetaDataList = new ContentMetaDataService(loggerForras.Object).GetContentMetaDataList()
		});

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

		var loggerRepo = new Mock<ILogger<AdminRepository>>();

		using var context = new AndrokatContext(GetDbContextOptions());

		var video = _fixture.Create<VideoContent>();

		var content1 = _fixture.Create<Content>();
		content1.Cim = "kurircím";
		content1.Tipus = (int)Forras.kurir;
		content1.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06";
		var ima = _fixture.Create<ImaContent>();

		var content = _fixture.Create<Content>();
		content.Tipus = (int)Forras.fokolare;
		content.Cim = "fokolareCim";
		content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-01 04:05:06";

		var systeminfo = _fixture.Create<SystemInfo>();
		systeminfo.Key = "radio";
		systeminfo.Value = "{ \"szentistvan\": \"url\"}";
		systeminfo.Id = 1;

        var radioMusor = _fixture.Create<RadioMusor>();
        radioMusor.Musor = "musor";
		radioMusor.Source = "source";
		radioMusor.Inserted = DateTime.Now.ToString("yyyy") + "-02-03T04:05:06";

        context.SystemInfo.Add(systeminfo);
		context.VideoContent.Add(video);
		context.Content.Add(content1);
		context.ImaContent.Add(ima);
		context.Content.Add(content);
		context.RadioMusor.Add(radioMusor);
		context.SaveChanges();

		var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, mapper);

		var model = new IndexModel(logger.Object, repo, authService.Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext),
			IsAdvent = true,
			IsNagyBojt = false
		};

		model.OnGet();
		model.AdminResult.Header.Should().Be(" | blog: <span style='color:red;'>NOT OK</span> #: 0 | video: #: 1 | ima: #: 1 | ");

        string expected = "<strong>Ma hiányzó anyagok</strong>" +
            "<br> 2 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Pio atya breviáriuma</a>" +
            "<br> 3 - <a href='https://www.szentgellertkiado.hu' target='_blank'>II. János Pál pápa breviáriuma</a>" +
            "<br> 4 - <a href='http://www.karmelitarend.hu' target='_blank'>Keresztes Szent János aranymondásai</a>" +
            "<br> 5 - <a href='http://karmelita.hu' target='_blank'>Kis Szent Teréz breviáriuma</a>" +
            "<br> 6 - <a href='https://www.evangelium365.hu' target='_blank'>Horváth István Sándor (evangélium365.hu)</a>" +
            "<br> 8 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Teréz Anya breviáriuma</a>" +
            "<br> 9 - <a href='https://www.magyarkurir.hu/ferenc-papa/ferenc-papa-twitter' target='_blank'>Ferenc pápa twitter üzenete</a>" +
            "<br> 10 - <a href='http://orszagutiferencesek.hu' target='_blank'>Advent</a>" +
            "<br> 11 - <a href='https://igenaptar.katolikus.hu' target='_blank'>Napi Ige és olvasmányok</a>" +
            "<br> 12 - <a href='http://szikrak.jezsuita.hu' target='_blank'>Loyolai Szent Ignác válogatott gondolatai</a>" +
            "<br> 13 - <a href='http://barsitelekmm.blogspot.hu' target='_blank'>Barsi és Telek - Magasság és mélység</a>" +
            "<br> 14 - <a href='https://www.evangelium365.hu' target='_blank'>Horváth István Sándor (evangélium365.hu)</a>" +
            "<br> 15 - <a href='https://open.spotify.com/show/4f2oWtBVw0jnTmeDrHh7ey' target='_blank'>Jezsuita napi-útra-való</a>" +
            "<br> 17 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Böjte Csaba testvér gondolatai</a>" +
            "<br> 19 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Kempis Tamás: Krisztus követése (válogatás)</a>" +
            "<br> 24 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Szeretet-újság</a>" +
            "<br> 25 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Humor</a>" +
            "<br> 28 - <a href='https://www.katolikusradio.hu' target='_blank'>Napi evangélium</a>" +
            "<br> 38 - <a href='https://www.youtube.com/c/BarsiBal%C3%A1zsOFM/videos' target='_blank'>Barsi Balázs atya prédikációi</a>" +
            "<br> 39 - <a href='https://www.youtube.com/PalferiOnline/videos' target='_blank'>Pál Feri atya prédikációi</a>" +
            "<br> 42 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Vianney Szent János breviáriuma</a>" +
            "<br> 45 - <a href='https://regnumchristi.hu' target='_blank'>Regnum Christi Mozgalom napi elmélkedése</a>" +
            "<br> 47 - <a href='https://www.taize.fr' target='_blank'>Taizé</a>" +
            "<br> 48 - <a href='http://www.szfvar.katolikus.hu/proima/prohaszka_breviarium' target='_blank'>Prohászka Ottokár breviáriuma</a>" +
            "<br> 50 - <a href='https://ciszterna.ocist.hu' target='_blank'>Szent Bernát idézetek minden napra</a>" +
            "<br> 52 - <a href='http://kairosz.hu' target='_blank'>Szalézi Szent Ferenc: Gondolatok minden napra</a>" +
            "<br> 57 - <a href='https://www.facebook.com/profile.php?id=100050238980182' target='_blank'>Varga László megyéspüspök gondolatai</a>" +
            "<br> 58 - <a href='https://www.szentgellertkiado.hu' target='_blank'>AJÁNDéKOZZ KÖNYVET</a>" +
            "<br> 60 - <a href='https://www.taize.fr' target='_blank'>Taizé napi imák</a>" +
            "<br> 61 - <a href='https://www.szentgellertkiado.hu' target='_blank'>Sienai Szent Katalin breviáriuma</a>" +
            "<br> 62 - <a href='https://www.libri.hu/konyv/beke-kiralynojenek-365-uzenete-medjugorjebol.html' target='_blank'>Medjugorje üzenetek</a>" +
            "<br><strong>Mai anyagok</strong><br> 7 - <a href='http://www.fokolare.hu' target='_blank'>Fokoláre: életige</a>" +
            "<br>[2025-02-01 04:05:06] - fokolareCim<br><br> 18 - <a href='https://www.magyarkurir.hu' target='_blank'>magyarkurir.hu</a>" +
            "<br>[2025-02-03 04:05:06] - kurircím<br><br>";        

		model.AdminAResult.MaiAnyagok.Should().Be(expected);

		model.AdminBResult.CountOfTipusok.Should().Be("7 Fokoláre: életige #: 1<br>18 magyarkurir.hu #: 1<br>21 Mai Szent #: 0<br>");
		model.AdminBResult.RadioList.Should().Be("source (" + DateTime.Now.ToString("yyyy") + "-02-03T04:05:06)<br>");
		model.AdminBResult.SysTable.Should().Be("[1] - radio<br>&nbsp;&nbsp;&nbsp;<a href=url target=\"_blank\">url</a>'<br>[3] - isNagyBojt - false<br>[2] - isAdvent - true<br>");
		model.AdminBResult.Radio.Should().Be("radio: <span style='color:red;'>NOT OK</span> #: 1");
	}

	[Fact]
	public void OnGet_Failed_Auth_Test()
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();

		var _logger = new Mock<ILogger<IndexModel>>();
		var adminRepository = new Mock<IAdminRepository>();
		var authService = new Mock<IAuthService>();
		authService.Setup(s => s.IsAuthenticated(It.IsAny<string>())).Returns(false);

		var model = new IndexModel(_logger.Object, adminRepository.Object, authService.Object)
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