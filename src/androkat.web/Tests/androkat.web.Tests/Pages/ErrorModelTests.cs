using androkat.domain.Enum;
using androkat.web.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.IO;
using System.Linq;

namespace androkat.web.Tests.Pages;

public class ErrorModelTests : BaseTest
{
	[Fact]
	public void ErrorModelTest_GeneralException()
	{		
		var logger = new Mock<ILogger<ErrorModel>>();

        var mockIFeatureCollection = new Mock<IFeatureCollection>();
        mockIFeatureCollection.Setup(p => p.Get<IExceptionHandlerPathFeature>())
            .Returns(new ExceptionHandlerFeature
            {
                Path = "/blabla",
                Error = new("blabla")
            });
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(p => p.Features).Returns(mockIFeatureCollection.Object);

        var (pageContext, tempData, actionContext) = GetPreStuff(httpContext.Object);

        var model = new ErrorModel(logger.Object)
		{
			PageContext = pageContext,
			TempData = tempData,
			Url = new UrlHelper(actionContext)
		};

		model.OnGet(null);

        logger.VerifyLogging("Error:  /blabla RequestId: (null)", LogLevel.Error, Times.Once());
    }

    [Fact]
    public void ErrorModelTest_FileNotFoundException()
    {
        var logger = new Mock<ILogger<ErrorModel>>();

        var mockIFeatureCollection = new Mock<IFeatureCollection>();
        mockIFeatureCollection.Setup(p => p.Get<IExceptionHandlerPathFeature>())
            .Returns(new ExceptionHandlerFeature
            {
                Path = "/blabla",
                Error = new FileNotFoundException("blabla")
            });
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(p => p.Features).Returns(mockIFeatureCollection.Object);
        httpContext.SetupGet(s => s.TraceIdentifier).Returns("1000");

        var (pageContext, tempData, actionContext) = GetPreStuff(httpContext.Object);

        var model = new ErrorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet(null);

        logger.VerifyLogging("Error: The file was not found. /blabla RequestId: 1000", LogLevel.Error, Times.Once());
    }

    [Fact]
    public void ErrorModelTest_Error_500()
    {
        var logger = new Mock<ILogger<ErrorModel>>();

        var mockIFeatureCollection = new Mock<IFeatureCollection>();
        mockIFeatureCollection.Setup(p => p.Get<IStatusCodeReExecuteFeature>())
            .Returns(new StatusCodeReExecuteFeature
            {
                OriginalPath = "/blabla",
                OriginalQueryString = string.Empty
            });
        mockIFeatureCollection.Setup(p => p.Get<IHttpRequestFeature>())
            .Returns(new HttpRequestFeature
            {
                Method = "GET",
                Protocol = string.Empty,
                Scheme = string.Empty
            });
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(p => p.Features).Returns(mockIFeatureCollection.Object);

        var (pageContext, tempData, actionContext) = GetPreStuff(httpContext.Object);

        var model = new ErrorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet(500);

        logger.VerifyLogging("Error - Path: /blabla, Query: , Code: 500, Method: GET, Scheme: , Protocol: ", LogLevel.Error, Times.Once());
    }
}