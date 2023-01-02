using androkat.application.Interfaces;
using androkat.application.Service;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace androkat.application.Tests.Services;

public class WarmupServiceTests : BaseTest
{
    private Mock<ICacheService> _cacheService = new Mock<ICacheService>();
    private Mock<ILogger<WarmupService>> logger = new Mock<ILogger<WarmupService>>();

    [Test]
    public void MainCacheFillUp_Happy()
    {
        var cache = GetIMemoryCache();

        var service = new WarmupService(cache, _cacheService.Object, logger.Object);
    }
}