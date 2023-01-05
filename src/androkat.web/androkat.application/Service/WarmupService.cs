using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

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

    public void MainCacheFillUp()
    {
        try
        {
            var result = _cacheService.MainCacheFillUp();
            _memoryCache.Set(CacheKey.MainCacheKey.ToString(), result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
            _logger.LogInformation("Warmup.MainCacheFillUp was called");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Warmup.MainCacheFillUp");
        }
    }
}