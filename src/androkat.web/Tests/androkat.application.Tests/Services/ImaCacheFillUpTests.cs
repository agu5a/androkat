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

public class ImaCacheFillUpTests : BaseTest
{
	[Fact]
	public void ImaCacheFillUp_Happy_test()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetImaToCache()).Returns(new List<ImaModel> { new(Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, string.Empty) });

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
		var res = cacheService.ImaCacheFillUp();

		res.Imak.Count.Should().Be(1);
		res.Inserted.ToString("yyyy-MM-dd").Should().Be("2012-01-03");
	}

	[Fact]
	public void ImaCacheFillUp_Throws_Exception()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetImaToCache()).Throws<Exception>();

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);

		Action act = () => cacheService.ImaCacheFillUp();
		act.Should().NotThrow<Exception>();
	}
}