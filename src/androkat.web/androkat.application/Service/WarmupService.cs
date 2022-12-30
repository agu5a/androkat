using androkat.application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace androkat.application.Service;

public class WarmupService : IWarmupService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<WarmupService> _logger;
    private readonly ICacheService _cacheService;

    public WarmupService(IMemoryCache memoryCache, ICacheService cacheService, ILogger<WarmupService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _cacheService = cacheService;
    }
}