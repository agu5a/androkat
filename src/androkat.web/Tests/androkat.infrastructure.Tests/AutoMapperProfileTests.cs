using androkat.domain.Model;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace androkat.infrastructure.Tests;

public class AutoMapperProfileTests
{
	[Test]
	public void Map_Video_VideoModel()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var nid = Guid.NewGuid();

		var result = mapper.Map<Video, VideoModel>(new Video
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

	[Test]
	public void Map_VideoSource_VideoSourceModel()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
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

	[Test]
	public void Map_Ima_ImaModel()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var nid = Guid.NewGuid();

		var result = mapper.Map<Ima, ImaModel>(new Ima
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

	[Test]
	public void Map_Napiolvaso_ContentDetailsModel()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var result = mapper.Map<Napiolvaso, ContentDetailsModel>(new Napiolvaso
		{
			Fulldatum = "2022-12-15 01:02:03"
		});

		result.Cim.Should().BeNull();
		result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-15 01:02:03");
	}

	[Test]
	public void Map_ContentDetailsModel_Napiolvaso()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var result = mapper.Map<ContentDetailsModel, Napiolvaso>(new ContentDetailsModel
		{
			Fulldatum = DateTime.Parse("2022-12-15 01:02:03")
		});

		result.Cim.Should().BeNull();
		result.Fulldatum.Should().Be("2022-12-15 01:02:03");
	}

	[Test]
	public void Map_FixContent_ContentDetailsModel()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var result = mapper.Map<FixContent, ContentDetailsModel>(new FixContent
		{
			Datum = "12-15"
		});

		result.Cim.Should().BeNull();
		result.FileUrl.Should().BeEmpty();
		result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy-") + "12-15 00:00:01");
	}

	[Test]
	public void Map_Maiszent_ContentDetailsModel()
	{
		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var result = mapper.Map<Maiszent, ContentDetailsModel>(new Maiszent
		{
			Datum = "12-15"
		});

		result.Cim.Should().BeNull();
		result.FileUrl.Should().BeEmpty();
		result.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy-") + "12-15 00:00:01");
	}
}