﻿using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using AutoMapper;
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
using Microsoft.Extensions.Logging;

namespace androkat.web.Tests;

public class BaseTest
{
    public static ContentMetaDataModel GetContentMetaDataModel(Forras? tipusId = null, string tipusNev = null, string forras = null,
        string link = null, string segedlink = null, string image = null)
    {
        return new ContentMetaDataModel(tipusId ?? Forras.pio, tipusNev ?? "", forras ?? "", link ?? "", segedlink ?? "", image ?? "");
    }

    public static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>(), (ILoggerFactory)null);
        return config.CreateMapper();
    }

    public static (PageContext pageContext, TempDataDictionary tempData, ActionContext actionContext) GetPreStuff(HttpContext context = null)
    {
        var httpContext = context ?? new DefaultHttpContext();
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

    public static Mock<IContentService> GetContentService(int tipus)
    {
        var contentService = new Mock<IContentService>();
        contentService.Setup(s => s.GetContentDetailsModelByNid(It.IsAny<Guid>(), (int)Forras.maiszent)).Returns(
            new ContentModel(
                 new ContentDetailsModel(Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                 GetContentMetaDataModel(image: "Image", link: "MetaLink", forras: "MetaForras", tipusId: (Forras)Enum.ToObject(typeof(Forras), tipus))
            ));
        contentService.Setup(s => s.GetContentDetailsModelByNid(It.IsAny<Guid>(), (int)Forras.szentbernat)).Returns(
            new ContentModel
            (
                new ContentDetailsModel(Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, "Forras"),
                GetContentMetaDataModel(image: "Image", link: "MetaLink", forras: "MetaForras", tipusId: (Forras)Enum.ToObject(typeof(Forras), tipus))
            ));
        contentService.Setup(s => s.GetContentDetailsModelByNid(It.IsAny<Guid>(), (int)Forras.ajanlatweb)).Returns(
            new ContentModel
            (
                new ContentDetailsModel(Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image: "Image", link: "MetaLink", forras: "MetaForras", tipusId: (Forras)Enum.ToObject(typeof(Forras), tipus))
            ));
        contentService.Setup(s => s.GetHumor()).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image: "Image")
            )
        });
        contentService.Setup(s => s.GetHome()).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image: "Image")
            )
        });
        contentService.Setup(s => s.GetAjanlat()).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image: "Image")
            )
        });
        contentService.Setup(s => s.GetSzentek()).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image: "Image")
            )
        });
        contentService.Setup(s => s.GetSzent()).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image : "Image")
            )
        });
        contentService.Setup(s => s.GetHirek(It.IsAny<int>())).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image : "Image")
            )
        });
        contentService.Setup(s => s.GetBlog(It.IsAny<int>())).Returns(new List<ContentModel>
        {
            new
            (
                new ContentDetailsModel (Guid.Empty, DateTime.MinValue, "Cim", string.Empty, tipus, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty),
                GetContentMetaDataModel(image : "Image")
            )
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

    public static DbContextOptions<AndrokatContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<AndrokatContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }
}
