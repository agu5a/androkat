using androkat.infrastructure.Model.SQLite;
using FluentAssertions;
using Xunit;
using System;

namespace androkat.infrastructure.Tests;

public class MaiszentTests
{
    [Fact]
    public void Maiszent_Happy()
    {
        var model = new Maiszent
        {
            Datum = "10-10"
        };
        model.Fulldatum.ToString("yyyy-MM-dd").Should().Be(DateTime.Now.ToString("yyyy-") + model.Datum);
    }

    [Fact]
    public void Maiszent_2024_And_02_29_Happy()
    {
        var model = new Maiszent
        {
            Datum = "02-29"
        };
        model.Fulldatum.ToString("yyyy-MM-dd").Should().Be("2024-" + model.Datum);
    }
}
