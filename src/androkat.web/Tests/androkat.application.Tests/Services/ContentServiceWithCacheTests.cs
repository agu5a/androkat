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
	public void GetVideoSource_FromCacheHappy()
	{
		Mock<ICacheRepository> repository = new();

		var now = DateTimeOffset.Parse("2012-02-03T04:05:06");

		var cache = GetIMemoryCache();

		var data = new List<VideoSourceModel>
		{
			new VideoSourceModel("UCF3mEbdkhZwjQE8reJHm4sg", "")
		};

		object result = new VideoCache { Video = new List<VideoModel>(), VideoSource = data };

		_ = cache.Set("VideoCacheKey", result);

		var cacheService = new CacheService(repository.Object, null, GetClock().Object);
		var contentService = new ContentService(cache, cacheService, GetAndrokatConfiguration());

		var videoPage = contentService.GetVideoSourcePage().ToList();

		videoPage[0].ChannelId.Should().Be("UCF3mEbdkhZwjQE8reJHm4sg");
		videoPage.Count.Should().Be(1);
	}
}
