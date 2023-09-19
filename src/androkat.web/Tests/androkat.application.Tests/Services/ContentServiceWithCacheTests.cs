using androkat.application.Service;
using androkat.domain;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class ContentServiceWithCacheTests : BaseTest
{
	[Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
	public void GetVideoSource_FromCacheHappy()
	{
		Mock<ICacheRepository> repository = new();

		var now = DateTimeOffset.Parse("2012-02-03T04:05:06");

		var cache = GetIMemoryCache();

		var data = new List<VideoSourceModel>
		{
			new("UCF3mEbdkhZwjQE8reJHm4sg", "")
		};

		object result = new VideoCache { Video = new List<VideoModel>(), VideoSource = data };

		_ = cache.Set("VideoCacheKey", result);

		var cacheService = new CacheService(repository.Object, null, GetClock().Object);
		var contentService = new ContentService(cache, cacheService, GetAndrokatConfiguration());

		var videoPage = contentService.GetVideoSourcePage().ToList();

		videoPage[0].ChannelId.Should().Be("UCF3mEbdkhZwjQE8reJHm4sg");
		videoPage.Count.Should().Be(1);
	}

    [Test]
    public void GetAudio_Happy()
    {
        Mock<ICacheRepository> repository = new();

        var now = DateTimeOffset.Parse("2012-02-03T04:05:06");

        var cache = GetIMemoryCache();

        object emptyResult = new MainCache { ContentDetailsModels = new List<ContentDetailsModel>() };
        var data = new List<ContentDetailsModel>
                {
                new(Guid.Parse("281cd115-1289-11ea-8aa1-cbeb38570c35"), now.DateTime, "cim1", "audiofile", 60, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)                        
                ,
                new(Guid.Parse("181cd115-1289-11ea-8aa1-cbeb38570c35"), now.AddDays(-1).DateTime, "cim2",
                "idezet", //ezt felül kell írja a FileUrl audio típusnál 
                60, DateTime.MinValue, string.Empty, string.Empty, "audiofile", string.Empty)                
            };

        object result = new MainCache
        {
            ContentDetailsModels = data
        };

        _ = cache.Set("MainCacheKey", result);
        _ = cache.Set("FixCacheKey", emptyResult);

        var cacheService = new CacheService(repository.Object, null, GetClock().Object);
        var service = new ContentService(cache, cacheService, GetAndrokatConfiguration());

        var audioRecords = service.GetAudio().ToList();

        string expected = "<div></div><div style=\"margin: 15px 0 0 0;\"><strong>Hangállomány meghallgatása</strong></div><div style=\"margin: 0 0 15px 0;\"><audio controls><source src=\"audiofile\" type=\"audio/mpeg\">Your browser does not support the audio element.</audio></div><div style=\"margin: 0 0 15px 0;word-break: break-all;\"><strong>Vagy a letöltése</strong>: <a href=\"audiofile\">audiofile</a></div>";

        audioRecords[0].Idezet.Should().Be(expected);
        audioRecords[1].Idezet.Should().Be(expected);
    }
}
