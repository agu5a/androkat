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

public class ImaCacheFillUpTests : BaseTest
{
	[Test]
	public void ImaCacheFillUp_Happy_test()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetImaToCache()).Returns(new List<ImaModel> { new ImaModel() });

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
		var res = cacheService.ImaCacheFillUp();

		Assert.That(res.Imak.Count(), Is.EqualTo(1));
		Assert.That(res.Inserted.ToString("yyyy-MM-dd"), Is.EqualTo("2012-01-03"));
	}

	[Test]
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