using androkat.domain.Enum;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class CacheRepositoryTests : BaseTest
{
    [Fact]
    public void GetHumorToCache_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
            var entity = new FixContent
            {
                Datum = "02-03",
                Tipus = (int)Forras.humor
            };
            context.FixContent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetHumorToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }

    [Theory]
    [InlineData((int)Forras.humor)]
    [InlineData((int)Forras.pio)]
    public void GetTodayFixContentToCache_Happy(int tipus)
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
            var entity = new FixContent
            {
                Datum = "02-03",
                Tipus = tipus
            };
            context.FixContent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetTodayFixContentToCache();
            if (tipus == (int)Forras.pio)
            {
            result.Count.Should().Be(1);
                result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
            }
            else
            {
            result.Count.Should().Be(0);
        }
    }

    [Fact]
	public void GetTodayFixContentToCache_No_Result()
	{
		var logger = new Mock<ILogger<CacheRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
			var entity = new FixContent
			{
				Datum = "02-03",
				Tipus = 1000 //invalid tipus
			};
			context.FixContent.Add(entity);
			context.SaveChanges();

			var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
			var result = repo.GetTodayFixContentToCache();
			result.Should().BeEmpty();			
		}

	[Fact]
    public void GetMaiSzentToCache_Ma_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
            var entity = new Maiszent
            {
                Datum = "02-03"
            };
            context.MaiSzent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetMaiSzentToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }

    [Fact]
    public void GetMaiSzentToCache_Tegnap_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
            var entity = new Maiszent
            {
                Datum = "02-02"
            };
            context.MaiSzent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetMaiSzentToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }

    [Fact]
    public void GetMaiSzentToCache_ElozoHonap_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
            var entity = new Maiszent
            {
                Datum = "01-31"
            };
            context.MaiSzent.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetMaiSzentToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + entity.Datum);
        }

    [Fact]
    public void GetContentDetailsModelToCache_Happy()
    {
        var logger = new Mock<ILogger<CacheRepository>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        var mapper = config.CreateMapper();

        var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
            var entity = new Content
            {
                Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03",
                Tipus = (int)Forras.audiohorvath
            };
            context.Content.Add(entity);
            context.SaveChanges();

            var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
            var result = repo.GetContentDetailsModelToCache();
            result.First().Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-02-03"));
        }        

	[Fact]
	public void GetVideoSourceToCache_Happy()
	{
		var logger = new Mock<ILogger<CacheRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
			var entity = new VideoContent
			{
				Forras = "Forras",
				ChannelId = "ChannelId"
			};
			context.VideoContent.Add(entity);
			context.SaveChanges();

			var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
			var result = repo.GetVideoSourceToCache();
			result.First().ChannelName.Should().Be("Forras");
			result.First().ChannelId.Should().Be("ChannelId");
		}

	[Fact]
	public void GetVideoToCache_Happy()
	{
		var logger = new Mock<ILogger<CacheRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
			var entity = new VideoContent
			{
				Forras = "Forras",
				ChannelId = "ChannelId",
                Inserted = clock.Object.Now.DateTime
			};
			context.VideoContent.Add(entity);
			context.SaveChanges();

			var repo = new CacheRepository(context, logger.Object, clock.Object, mapper);
			var result = repo.GetVideoToCache();
			result.First().Forras.Should().Be("Forras");
			result.First().Inserted.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-02-03"));
		}
	}