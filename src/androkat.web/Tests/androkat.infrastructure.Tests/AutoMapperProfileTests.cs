using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Xunit;
using System;
using Microsoft.Extensions.Logging;

namespace androkat.infrastructure.Tests;

public class AutoMapperProfileTests
{
	[Fact]
	public void Map_Video_VideoModel()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();

		var nid = Guid.NewGuid();

		var result = mapper.Map<VideoContent, VideoModel>(new VideoContent
		{
			Date = "2022-12-15 01:02:03",
			Cim = "cím",
			ChannelId = "ChannelId",
			Nid = nid,
			VideoLink = "VideoLink"
		});

		result.Cim.Should().Be("cím");
		result.Nid.Should().Be(nid);
		result.ChannelId.Should().Be("ChannelId");
		result.VideoLink.Should().Be("VideoLink");
		result.Date.Should().Be("2022-12-15 01:02:03");
	}

	[Fact]
	public void Map_VideoSource_VideoSourceModel()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();

		var nid = Guid.NewGuid();

		var result = mapper.Map<VideoSource, VideoSourceModel>(new VideoSource
		{
			ChannelId = "ChannelId",
			ChannelName = "ChannelName"
		});

		result.ChannelId.Should().Be("ChannelId");
		result.ChannelName.Should().Be("ChannelName");
	}

	[Fact]
	public void Map_Ima_ImaModel()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();

		var nid = Guid.NewGuid();

		var result = mapper.Map<ImaContent, ImaModel>(new ImaContent
		{
			Datum = DateTime.Parse("2022-12-15 01:02:03"),
			Cim = "cím",
			Csoport = "csoport",
			Nid = nid,
			Szoveg = "szöveg"
		});

		result.Cim.Should().Be("cím");
		result.Datum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
	}

	[Fact]
	public void Map_Content_ContentDetailsModel()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();

		var result = mapper.Map<Content, ContentDetailsModel>(new Content
		{
			Fulldatum = "2022-12-15 01:02:03"
		});

		result.Cim.Should().BeEmpty();
		result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
	}

	[Fact]
	public void Map_ContentDetailsModel_Content()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();
		var result = mapper.Map<ContentDetailsModel, Content>(new ContentDetailsModel(Guid.Empty, DateTime.Parse("2022-12-15 01:02:03"), null, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
		);
		result.Cim.Should().BeNull();
		result.Fulldatum.Should().Be("2022-12-15 01:02:03");
	}

	[Fact]
	public void Map_FixContent_ContentDetailsModel()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();

		var result = mapper.Map<FixContent, ContentDetailsModel>(new FixContent
		{
			Datum = "12-15"
		});

		result.Cim.Should().BeEmpty();
		result.FileUrl.Should().BeNull();
		result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy-") + "12-15 00:00:00");
	}

	[Fact]
	public void Map_Maiszent_ContentDetailsModel()
	{
		using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
		var config = new MapperConfiguration(cfg =>
		{
			cfg.LicenseKey = "<License Key Here>";
			cfg.AddProfile<AutoMapperProfile>();
		}, loggerFactory);
		var mapper = config.CreateMapper();

		var result = mapper.Map<Maiszent, ContentDetailsModel>(new Maiszent
		{
			Datum = "12-15"
		});

		result.Cim.Should().BeEmpty();
		result.FileUrl.Should().BeNull();
		result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy-") + "12-15 00:00:00");
	}
}