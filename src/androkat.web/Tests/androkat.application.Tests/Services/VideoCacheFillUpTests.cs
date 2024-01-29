using androkat.application.Interfaces;
using androkat.application.Service;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Tests.Services;

public class VideoCacheFillUpTests : BaseTest
{
	[Fact]
	public void VideoCacheFillUp_Happy_test()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetVideoToCache()).Returns(new List<VideoModel>
		{
			new(Guid.Empty, string.Empty, "https://www.youtube.com/embed/OnCW6hg5CdQ", string.Empty, string.Empty, string.Empty, string.Empty, DateTime.MinValue),
			new(Guid.Empty, string.Empty, "https://www.youtube.com/watch?v=OnCW6hg5CdQ", string.Empty, string.Empty, string.Empty, string.Empty, DateTime.MinValue)
		});

		cacheRepository.Setup(s => s.GetVideoSourceToCache()).Returns(new List<VideoSourceModel>());

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
		var res = cacheService.VideoCacheFillUp();

		res.Video.Count.Should().Be(2);
		res.Video.ElementAt(0).VideoLink.Should().NotContain("embed");
		res.Video.ElementAt(1).VideoLink.Should().NotContain("embed");
		res.Inserted.ToString("yyyy-MM-dd").Should().Be("2012-01-03");
	}

	[Fact]
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