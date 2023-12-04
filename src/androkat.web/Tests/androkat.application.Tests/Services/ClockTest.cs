using androkat.application.Service;
using FluentAssertions;
using System;
using Xunit;

namespace androkat.application.Tests.Services;

public class ClockTest
{
    [Fact]
    public void DeleteOldRowsTest()
    {
        var context = new Clock();
        var res = context.Now;

        (res <= DateTimeOffset.UtcNow.AddHours(2)).Should().BeTrue();
    }
}
