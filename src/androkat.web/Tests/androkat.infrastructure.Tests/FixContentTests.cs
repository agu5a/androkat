using androkat.infrastructure.Model.SQLite;
using FluentAssertions;
using Xunit;
using System;

namespace androkat.infrastructure.Tests;

public class FixContentTests
{
	[Fact]
	public void FixContent_Happy()
	{
		var model = new FixContent
		{
			Datum = "10-10"
		};
		model.Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.UtcNow.ToString("yyyy-") + model.Datum);
	}

    [Fact]
    public void FixContent_2023_And_02_29_Happy()
    {
        var model = new FixContent
        {
            Datum = "02-29"
        };
        model.Fulldatum.ToString("yyyy-MM-dd").Should().Be("2024-" + model.Datum);
    }

    [Fact]
	public void FixContent_FallBack()
	{
		var model = new FixContent
		{
			Datum = ""
		};
		model.Fulldatum.Should().Be(DateTime.MinValue);
	}
}
