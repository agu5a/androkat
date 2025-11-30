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

public class TukorModelTests
{
    [Fact]
    public void TukorModel_OnGet_WithEmptySession_DoesNotSetCheckboxes()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.TukorModel>>();

        var model = new web.Pages.Gyonas.TukorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.cb11.Should().BeFalse();
        model.cb12.Should().BeFalse();
    }

    [Fact]
    public void TukorModel_OnGet_WithValidSession_SetsCheckboxes()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.TukorModel>>();

        var dic = CreateAllFalseCheckboxDictionary();
        dic["cb11"] = true;
        dic["cb21"] = true;
        httpContext.Session.SetString("conf", JsonSerializer.Serialize(dic));

        var model = new web.Pages.Gyonas.TukorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.cb11.Should().BeTrue();
        model.cb21.Should().BeTrue();
        model.cb12.Should().BeFalse();
    }

    [Fact]
    public void TukorModel_OnGet_WithInvalidSession_LogsError()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.TukorModel>>();

        httpContext.Session.SetString("conf", "invalid json");

        var model = new web.Pages.Gyonas.TukorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();

        model.cb11.Should().BeFalse();
    }

    [Fact]
    public void TukorModel_OnPostSave_SavesCheckboxesToSession()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.TukorModel>>();

        var model = new web.Pages.Gyonas.TukorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            cb11 = true,
            cb21 = true,
            cb31 = false
        };

        model.OnPostSave();

        var savedJson = httpContext.Session.GetString("conf");
        savedJson.Should().NotBeNullOrEmpty();
        var savedDic = JsonSerializer.Deserialize<Dictionary<string, bool>>(savedJson);
        savedDic["cb11"].Should().BeTrue();
        savedDic["cb21"].Should().BeTrue();
        savedDic["cb31"].Should().BeFalse();
    }

    [Fact]
    public void TukorModel_OnPostClear_ClearsSessionAndRedirects()
    {
        var (pageContext, tempData, actionContext, httpContext) = GetPreStuffWithSession();
        var logger = new Mock<ILogger<web.Pages.Gyonas.TukorModel>>();

        httpContext.Session.SetString("conf", "some data");

        var model = new web.Pages.Gyonas.TukorModel(logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        var result = model.OnPostClear();

        result.Should().BeOfType<RedirectResult>();
        ((RedirectResult)result).Url.Should().Be("/gyonas/tukor");
        httpContext.Session.GetString("conf").Should().BeEmpty();
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

internal class SessionFeature : ISessionFeature
{
    public ISession Session { get; set; }
}

internal class TestSession : ISession
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
