using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.Pages;

public class GyonasModelTests
{
    [Fact]
    public void GyonasModel_OnGet_WithEmptySession_ReturnsEmptyDiv()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.GyonasModel>>();

        var model = new web.Pages.Gyonas.GyonasModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.Parancsok.Should().Be("<div id=\"sins\"></div>");
    }

    [Fact]
    public void GyonasModel_OnGet_WithValidSession_GeneratesHtml()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.GyonasModel>>();

        var dic = CreateAllFalseCheckboxDictionary();
        dic["cb11"] = true;
        httpContext.Session.SetString("conf", JsonSerializer.Serialize(dic));

        var model = new web.Pages.Gyonas.GyonasModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.Parancsok.Should().Contain("<li><strong>");
        model.Parancsok.Should().Contain("Áldoztam");
    }

    [Fact]
    public void GyonasModel_OnGet_WithMultipleChecked_GeneratesMultipleItems()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.GyonasModel>>();

        var dic = CreateAllFalseCheckboxDictionary();
        dic["cb11"] = true;
        dic["cb21"] = true;
        dic["cb31"] = true;
        httpContext.Session.SetString("conf", JsonSerializer.Serialize(dic));

        var model = new web.Pages.Gyonas.GyonasModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.Parancsok.Should().Contain("Áldoztam");
        model.Parancsok.Should().Contain("Esküdöztem");
        model.Parancsok.Should().Contain("Vasárnap");
    }

    [Fact]
    public void GyonasModel_OnGet_WithInvalidSession_LogsErrorAndReturnsEmptyDiv()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.GyonasModel>>();

        httpContext.Session.SetString("conf", "invalid json");

        var model = new web.Pages.Gyonas.GyonasModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.Parancsok.Should().Be("<div id=\"sins\"></div>");
    }

    [Fact]
    public void GyonasModel_OnGet_WithAllCommandments_GeneratesCorrectHtml()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.GyonasModel>>();

        var dic = CreateAllFalseCheckboxDictionary();
        // Select one from each commandment group
        dic["cb11"] = true;  // 1st commandment
        dic["cb21"] = true;  // 2nd commandment
        dic["cb31"] = true;  // 3rd commandment
        dic["cb41"] = true;  // 4th commandment
        dic["cb51"] = true;  // 5th commandment
        dic["cb61"] = true;  // 6th commandment
        dic["cb71"] = true;  // 7th commandment
        dic["cb81"] = true;  // 8th commandment
        dic["cb91"] = true;  // 9th commandment
        dic["cb101"] = true; // 10th commandment
        httpContext.Session.SetString("conf", JsonSerializer.Serialize(dic));

        var model = new web.Pages.Gyonas.GyonasModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.Parancsok.Should().StartWith("<div id=\"sins\">");
        model.Parancsok.Should().EndWith("</div>");
        model.Parancsok.Should().Contain("<li><strong>");
    }

    private static Dictionary<string, bool> CreateAllFalseCheckboxDictionary()
    {
        return new Dictionary<string, bool>
        {
            { "cb11", false }, { "cb12", false }, { "cb13", false }, { "cb14", false }, { "cb15", false },
            { "cb21", false }, { "cb22", false }, { "cb23", false }, { "cb24", false }, { "cb25", false },
            { "cb31", false }, { "cb32", false }, { "cb33", false }, { "cb34", false }, { "cb35", false },
            { "cb41", false }, { "cb42", false }, { "cb43", false }, { "cb44", false },
            { "cb51", false }, { "cb52", false }, { "cb53", false }, { "cb54", false },
            { "cb61", false }, { "cb62", false }, { "cb63", false }, { "cb64", false },
            { "cb71", false }, { "cb72", false }, { "cb73", false }, { "cb74", false }, { "cb75", false },
            { "cb81", false }, { "cb82", false }, { "cb83", false }, { "cb84", false }, { "cb85", false },
            { "cb91", false }, { "cb92", false }, { "cb93", false },
            { "cb101", false }, { "cb102", false }, { "cb103", false }, { "cb104", false }
        };
    }

    private static (PageContext pageContext, TempDataDictionary tempData, ActionContext actionContext, DefaultHttpContext httpContext) GetPreStuffWithSession()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = new TestSession() });

        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext)
        {
            ViewData = viewData
        };

        return (pageContext, tempData, actionContext, httpContext);
    }
}

internal class SessionFeatureGyonas : ISessionFeature
{
    public ISession Session { get; set; }
}

internal class TestSessionGyonas : ISession
{
    private readonly Dictionary<string, byte[]> _store = new();

    public string Id => "test-session";
    public bool IsAvailable => true;
    public IEnumerable<string> Keys => _store.Keys;

    public void Clear() => _store.Clear();

    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void Remove(string key) => _store.Remove(key);

    public void Set(string key, byte[] value) => _store[key] = value;

    public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
}
