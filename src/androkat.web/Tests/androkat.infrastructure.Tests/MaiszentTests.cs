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
}
