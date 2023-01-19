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

public class EgyebCacheFillUpTests : BaseTest
{
	[Test]
	public void EgyebCacheFillUp_Happy_test()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetHirekBlogokToCache()).Returns(new List<ContentDetailsModel>
		{
			new ContentDetailsModel()
		});       
        
		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);
		var res = cacheService.EgyebCacheFillUp();

		Assert.That(res.Egyeb.Count(), Is.EqualTo(1));
        Assert.That(res.Inserted.ToString("yyyy-MM-dd"), Is.EqualTo("2012-01-03"));
	}

	[Test]
	public void EgyebCacheFillUp_Throws_Exception()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = new Mock<IClock>();
		clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse("2012-01-03T04:05:06"));

		var cacheRepository = new Mock<ICacheRepository>();
		cacheRepository.Setup(s => s.GetHirekBlogokToCache()).Throws<Exception>();

		var cacheService = new CacheService(cacheRepository.Object, new Mock<ILogger<CacheService>>().Object, clock.Object);

		Action act = () => cacheService.EgyebCacheFillUp();
		act.Should().NotThrow<Exception>();
	}
}