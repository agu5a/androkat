using androkat.infrastructure.Model.SQLite;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace androkat.infrastructure.Tests;

public class MaiszentTests
{
	[Test]
	public void Maiszent_Happy()
	{
		var model = new Maiszent
		{
			Datum = "10-10"
		};
		model.FullDate.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + model.Datum);
	}

    [Test]
    public void Maiszent_2023_And_02_29_Happy()
    {
        var model = new Maiszent
        {
            Datum = "02-29"
        };
        model.FullDate.ToString("yyyy-MM-dd").Should().Be("2024-" + model.Datum);
    }
}
