using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;

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

    public ImaResponse GetImaByDate(string date, ImaCache imaCache)
    {
        _ = DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("hu-HU"), out DateTime datum);

        string key = CacheKey.ImaResponseCacheKey + "_" + datum.ToString("yyyy-MM-dd_HH:mm:ss ");
        var result = GetCache<ImaResponse>(key);
        if (result is not null)
            return result;

        imaCache = GetCache(CacheKey.ImaCacheKey.ToString(), () => { return _cacheService.ImaCacheFillUp(); });
        result = _apiService.GetImaByDate(date, imaCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public IReadOnlyCollection<ContentResponse> GetContentByTipusAndNid(int tipus, Guid n, MainCache mainCache)
    {
        string key = CacheKey.ContentResponseCacheKey + "_" + tipus + "_" + n;
        var result = GetCache<IReadOnlyCollection<ContentResponse>>(key);
        if (result is not null)
            return result;

        mainCache = GetCache(CacheKey.MainCacheKey.ToString(), () => { return _cacheService.MainCacheFillUp(); });
        result = _apiService.GetContentByTipusAndNid(tipus, n, mainCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public IReadOnlyCollection<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache)
    {
        string key = CacheKey.RadioResponseCacheKey + "_" + s;
        var result = GetCache<IReadOnlyCollection<RadioMusorResponse>>(key);
        if (result is not null)
            return result;

        bookRadioSysCache = GetCache(CacheKey.BookRadioSysCacheKey.ToString(), () => { return _cacheService.BookRadioSysCacheFillUp(); });
        result = _apiService.GetRadioBySource(s, bookRadioSysCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public IReadOnlyCollection<SystemDataResponse> GetSystemData(BookRadioSysCache bookRadioSysCache)
    {
        bookRadioSysCache = GetCache(CacheKey.BookRadioSysCacheKey.ToString(), () => { return _cacheService.BookRadioSysCacheFillUp(); });
        var result = _apiService.GetSystemData(bookRadioSysCache);
        return result;
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

    public string GetVideoForWebPage(string f, int offset, VideoCache videoCache)
    {
        string key = CacheKey.VideoResponseCacheKey + "_" + f + "_" + offset;
        var result = GetCache<string>(key);
        if (!string.IsNullOrWhiteSpace(result))
            return result;

        videoCache = GetCache(CacheKey.VideoCacheKey.ToString(), () => { return _cacheService.VideoCacheFillUp(); });
        result = _apiService.GetVideoForWebPage(f, offset, videoCache);
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