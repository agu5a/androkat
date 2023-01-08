using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class VideoCacheFillUpTests : BaseTest
{
	[Test]
	public void VideoCacheFillUp_Happy_test()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetVideoToCache()).Returns(new List<VideoModel> 
		{
			new VideoModel{ VideoLink = "https://www.youtube.com/embed/OnCW6hg5CdQ" },
			new VideoModel{ VideoLink = "https://www.youtube.com/watch?v=OnCW6hg5CdQ" }
		});

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
		var res = cacheService.VideoCacheFillUp();

		Assert.That(res.Video.Count(), Is.EqualTo(2));
		res.Video[0].VideoLink.Should().NotContain("embed");
		res.Video[1].VideoLink.Should().NotContain("embed");
		Assert.That(res.Inserted.ToString("yyyy-MM-dd"), Is.EqualTo("2012-01-03"));
	}

	[Test]
	public void VideoCacheFillUp_Throws_Exception()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetVideoToCache()).Throws<Exception>();

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);

		Action act = () => cacheService.VideoCacheFillUp();
		act.Should().NotThrow<Exception>();
	}
}