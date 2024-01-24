using androkat.application.Interfaces;
using androkat.domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace androkat.web.Tests.Pages.Partner;

public class PartnerModelsTests : BaseTest
{
	[Fact]
	public void PartnerIndexModelTest()
	{
		var (pageContext, tempData, actionContext) = GetPreStuff();

		var _partnerRepository = new Mock<IPartnerRepository>();
		var _logger = new Mock<ILogger<web.Pages.Partner.IndexModel>>();
		var _iClock = new Mock<IClock>();

		var model = new web.Pages.Partner.IndexModel(_logger.Object, _partnerRepository.Object, _iClock.Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext)
		};

		dynamic res = model.OnGet(1, "nid");
		string url = res.Url;
		url.Should().Be("/");
	}
}