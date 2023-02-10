using androkat.infrastructure.Model.SQLite;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace androkat.infrastructure.Tests;

public class FixContentTests
{
	[Test]
	public void FixContent_Happy()
	{
		var model = new FixContent
		{
			Fulldatum = "10-10"
		};
		model.FullDate.ToString("yyyy-MM-dd").Should().Be(DateTime.UtcNow.ToString("yyyy-") + model.Fulldatum);
	}

    [Test]
    public void FixContent_2023_And_02_29_Happy()
    {
        var model = new FixContent
        {
            Fulldatum = "02-29"
        };
        model.FullDate.ToString("yyyy-MM-dd").Should().Be("2024-" + model.Fulldatum);
	}

	[Test]
	public void FixContent_FallBack()
	{
		var model = new FixContent
		{
			Fulldatum = ""
		};
		model.FullDate.Should().Be(DateTime.MinValue);
	}
}
