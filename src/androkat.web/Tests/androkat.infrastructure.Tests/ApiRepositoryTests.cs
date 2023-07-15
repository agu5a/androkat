using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class ApiRepositoryTests : BaseTest
{
	[Test]
	public void UpdateRadioMusor_NotFound()
	{
		var logger = new Mock<ILogger<ApiRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.UpdateRadioMusor(new domain.Model.RadioMusorModel(Guid.Empty, "Source", string.Empty, string.Empty));
        result.Should().Be(false);
        context.RadioMusor.FirstOrDefault(f => f.Source == "Source").Should().BeNull();
    }

	[Test]
	public void UpdateRadioMusor_Happy()
	{
		var logger = new Mock<ILogger<ApiRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new RadioMusor
        {
            Source = "Source",
            Inserted = "2023-01-10",
            Musor = "Műsor",
            Nid = Guid.NewGuid()
        };
        context.RadioMusor.Add(entity);
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.UpdateRadioMusor(new domain.Model.RadioMusorModel(Guid.Empty, "Source", "Műsor 2", "2023-01-11"));
        result.Should().Be(true);
        var radio = context.RadioMusor.FirstOrDefault(f => f.Source == "Source");
        radio.Musor.Should().Be("Műsor 2");
        radio.Inserted.Should().Be("2023-01-11");
    }

	[TestCase(1, "cím1", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B", 1, "cím1", "AA4E35F9-0875-49E9-8A19-67AD429BE747")]
	[TestCase(1, "cím1", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B", 1, "cím2", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B")]
	public void AddContentDetailsModel_Conflict(int tipusDb, string cimDb, string guidDb, int tipus, string cim, string guid)
	{
		var logger = new Mock<ILogger<ApiRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

		var nidDb = Guid.Parse(guidDb);
		var nid = Guid.Parse(guid);
        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = tipusDb,
            Cim = cimDb,
            Nid = nidDb
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.AddContentDetailsModel(new domain.Model.ContentDetailsModel(nid, DateTime.MinValue, cim, string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        );
        result.Should().Be(false);
        context.Content.FirstOrDefault(f => f.Nid == nid && f.Cim == cim).Should().BeNull();
    }

	[TestCase(1, "cím1", "9E0BFF6C-619D-4A2A-884B-7A36F6E7C15B", 1, "cím2", "AA4E35F9-0875-49E9-8A19-67AD429BE747")]
	public void AddContentDetailsModel_Happy(int tipusDb, string cimDb, string guidDb, int tipus, string cim, string guid)
	{
		var logger = new Mock<ILogger<ApiRepository>>();

		var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
		var mapper = config.CreateMapper();

		var clock = GetToday();

		var nidDb = Guid.Parse(guidDb);
		var nid = Guid.Parse(guid);
        using var context = new AndrokatContext(GetDbContextOptions());
        var entity = new Content
        {
            Tipus = tipusDb,
            Cim = cimDb,
            Nid = nidDb
        };
        context.Content.Add(entity);
        context.SaveChanges();

        var repo = new ApiRepository(context, clock.Object, mapper);
        var result = repo.AddContentDetailsModel(new domain.Model.ContentDetailsModel(nid, DateTime.MinValue, cim, string.Empty, tipus, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
        );
        result.Should().Be(true);
        context.Content.FirstOrDefault(f => f.Nid == nid && f.Cim == cim).Should().NotBeNull();
    }
}
