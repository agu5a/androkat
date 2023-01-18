using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace androkat.web.Tests;

public static class MockHelper
{
    public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel, Times times)
    {
        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            times);

        return logger;
    }

    public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel, Times times)
    {
        Func<object, Type, bool> state = (v, t) =>
        {
            var orig = v.ToString();
            bool result = orig == expectedMessage;
            return result;
        };

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            times);

        return logger;
    }
}
