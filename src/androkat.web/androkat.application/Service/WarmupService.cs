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
			_memoryCache.Set(CacheKey.MainCacheKey.ToString(), _cacheService.MainCacheFillUp(),
				new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
			_logger.LogInformation("{name} was called", nameof(MainCacheFillUp));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: {name}", nameof(MainCacheFillUp));
        }
    }

    public void BookRadioSysCache()
    {
        try
        {
            var result = _cacheService.BookRadioSysCacheFillUp();
            _memoryCache.Set(CacheKey.BookRadioSysCacheKey.ToString(), result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
            _logger.LogInformation("Warmup.BookRadioSysCache was called");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Warmup.BookRadioSysCache");
        }
    }

    public void ImaCacheFillUp()
    {
        try
        {
            _memoryCache.Set(CacheKey.ImaCacheKey.ToString(), _cacheService.ImaCacheFillUp(),
				new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
			_logger.LogInformation("{name} was called", nameof(ImaCacheFillUp));
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, "Exception: {name}", nameof(ImaCacheFillUp));
        }
    }

    public void VideoCacheFillUp()
    {
        try
        {
            var result = _cacheService.VideoCacheFillUp();
            _memoryCache.Set(CacheKey.VideoCacheKey.ToString(), result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
			_logger.LogInformation("{name} was called", nameof(VideoCacheFillUp));
        }
        catch (Exception ex)
        {
			_logger.LogError(ex, "Exception: {name}", nameof(VideoCacheFillUp));
        }
    }
}