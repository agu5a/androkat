using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Service;

public class ApiServiceCacheDecorate : IApiService
{
    private readonly IApiService _apiService;
    private readonly IMemoryCache _memoryCache;
    private readonly ICacheService _cacheService;

    public ApiServiceCacheDecorate(
        IApiService apiService, 
        IMemoryCache memoryCache, 
        ICacheService cacheService)
    {
        _apiService = apiService;
        _memoryCache = memoryCache;
        _cacheService = cacheService;
    }

    public IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset, VideoCache videoCache)
    {
        string key = CacheKey.VideoResponseCacheKey + "_" + offset;
        var result = GetCache<IReadOnlyCollection<VideoResponse>>(key);
        if (result is not null)
            return result;

        videoCache = GetCache(CacheKey.VideoCacheKey.ToString(), () => { return _cacheService.VideoCacheFillUp(); });
        result = _apiService.GetVideoByOffset(offset, videoCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }    

    private C GetCache<C>(string key)
    {
        if (_memoryCache.TryGetValue(key, out var result) && result is C cached)
            return cached;

        return default;
    }

    private C GetCache<C>(string key, Func<C> function)
    {
        var cached = GetCache<C>(key);
        if (cached is not null)
            return cached;

        var res = function();
        if (res is not null)
            _memoryCache.Set(key, res, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return res;
    }
}