using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;

namespace androkat.web.Tests;

public class BaseTest
{
    public static (PageContext pageContext, TempDataDictionary tempData, ActionContext actionContext) GetPreStuff()
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

    public static Mock<IContentService> GetContentService(int? tipus = null)
    {
        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetHome()).Returns(new List<ContentModel>
        {
            new ContentModel
            {
                ContentDetails = new ContentDetailsModel { Cim = "Cim", Tipus = tipus ?? (int)Forras.papaitwitter, Img = "Image" },
                MetaData = new ContentMetaDataModel { Image = "Image" }
            }
        });
        contentService.Setup(s => s.GetAjanlat()).Returns(new List<ContentModel>
        {
            new ContentModel
            {
                ContentDetails = new ContentDetailsModel { Cim = "Cim", Tipus = tipus ?? (int)Forras.ajanlatweb, Img = "Image" },
                MetaData = new ContentMetaDataModel { Image = "Image" }
            }
        });
        contentService.Setup(s => s.GetSzentek()).Returns(new List<ContentModel>
        {
            new ContentModel
            {
                ContentDetails = new ContentDetailsModel { Cim = "Cim", Tipus = tipus ?? (int)Forras.pio, Img = "Image" },
                MetaData = new ContentMetaDataModel { Image = "Image" }
            }
        });
        contentService.Setup(s => s.GetSzent()).Returns(new List<ContentModel>
        {
            new ContentModel
            {
                ContentDetails = new ContentDetailsModel { Cim = "Cim", Tipus = tipus ?? (int)Forras.maiszent, Img = "Image" },
                MetaData = new ContentMetaDataModel { Image = "Image" }
            }
        });

        return contentService;
    }

    public static IMemoryCache GetIMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        return memoryCache;
    }

    public DbContextOptions<AndrokatContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<AndrokatContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }
}
