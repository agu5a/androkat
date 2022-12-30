using androkat.application.Service;
using NUnit.Framework;
using System;

namespace androkat.application.Tests.Services;

public class ClockTest
{
    [Test]
    public void DeleteOldRowsTest()
    {
        var context = new Clock();
        var res = context.Now;

        Assert.IsTrue(res <= DateTimeOffset.UtcNow.AddHours(2));
    }
}
