using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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

    private static string GenerateETag<T>(T content)
    {
        var json = JsonSerializer.Serialize(content);
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(json));
        return $"\"{Convert.ToHexString(hash).ToLowerInvariant()}\"";
    }

    public IReadOnlyCollection<ContentResponse> GetContentByTipusAndId(int tipus, Guid id, BookRadioSysCache bookRadioSysCache, MainCache mainCache)
    {
        if (AndrokatConfiguration.BlogNewsContentTypeIds().Contains(tipus) || tipus == (int)Forras.book)
        {
            return GetEgyebOlvasnivaloByForrasAndNid(tipus, id, bookRadioSysCache, mainCache);
        }

        return GetContentByTipusAndNid(tipus, id, mainCache);
    }

    public IReadOnlyCollection<ContentResponse> GetEgyebOlvasnivaloByForrasAndNid(int tipus, Guid n, BookRadioSysCache bookRadioSysCache, MainCache mainCache)
    {
        var key = CacheKey.EgyebOlvasnivaloResponseCacheKey + "_" + tipus + "_" + n;
        var result = GetCache<IReadOnlyCollection<ContentResponse>>(key);
        if (result is not null)
        {
            return result;
        }

        bookRadioSysCache = GetCache(CacheKey.BookRadioSysCacheKey.ToString(), () => _cacheService.BookRadioSysCacheFillUp());
        mainCache = GetCache(CacheKey.MainCacheKey.ToString(), () => _cacheService.MainCacheFillUp());
        result = _apiService.GetEgyebOlvasnivaloByForrasAndNid(tipus, n, bookRadioSysCache, mainCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        // Cache ETag alongside response
        var etagKey = key + "_etag";
        var etag = GenerateETag(result);
        _memoryCache.Set(etagKey, etag, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public ImaResponse GetImaByDate(string date, ImaCache imaCache)
    {
        _ = DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("hu-HU"), out var datum);

        var key = CacheKey.ImaResponseCacheKey + "_" + datum.ToString("yyyy-MM-dd_HH:mm:ss ");
        var result = GetCache<ImaResponse>(key);
        if (result is not null)
        {
            return result;
        }

        imaCache = GetCache(CacheKey.ImaCacheKey.ToString(), () => _cacheService.ImaCacheFillUp());
        result = _apiService.GetImaByDate(date, imaCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public IReadOnlyCollection<ContentResponse> GetContentByTipusAndNid(int tipus, Guid n, MainCache mainCache)
    {
        var key = CacheKey.ContentResponseCacheKey + "_" + tipus + "_" + n;
        var result = GetCache<IReadOnlyCollection<ContentResponse>>(key);
        if (result is not null)
        {
            return result;
        }

        mainCache = GetCache(CacheKey.MainCacheKey.ToString(), () => _cacheService.MainCacheFillUp());
        result = _apiService.GetContentByTipusAndNid(tipus, n, mainCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        // Cache ETag alongside response
        var etagKey = key + "_etag";
        var etag = GenerateETag(result);
        _memoryCache.Set(etagKey, etag, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public IReadOnlyCollection<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache)
    {
        var key = CacheKey.RadioResponseCacheKey + "_" + s;
        var result = GetCache<IReadOnlyCollection<RadioMusorResponse>>(key);
        if (result is not null)
        {
            return result;
        }

        bookRadioSysCache = GetCache(CacheKey.BookRadioSysCacheKey.ToString(), () => _cacheService.BookRadioSysCacheFillUp());
        result = _apiService.GetRadioBySource(s, bookRadioSysCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public IReadOnlyCollection<SystemDataResponse> GetSystemData(BookRadioSysCache bookRadioSysCache)
    {
        bookRadioSysCache = GetCache(CacheKey.BookRadioSysCacheKey.ToString(), () => _cacheService.BookRadioSysCacheFillUp());
        var result = _apiService.GetSystemData(bookRadioSysCache);
        return result;
    }

    public IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset, VideoCache videoCache)
    {
        var key = CacheKey.VideoResponseCacheKey + "_" + offset;
        var result = GetCache<IReadOnlyCollection<VideoResponse>>(key);
        if (result is not null)
        {
            return result;
        }

        videoCache = GetCache(CacheKey.VideoCacheKey.ToString(), () => _cacheService.VideoCacheFillUp());
        result = _apiService.GetVideoByOffset(offset, videoCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    public string GetVideoForWebPage(string f, int offset, VideoCache videoCache)
    {
        var key = CacheKey.VideoResponseCacheKey + "_" + f + "_" + offset;
        var result = GetCache<string>(key);
        if (!string.IsNullOrWhiteSpace(result))
        {
            return result;
        }

        videoCache = GetCache(CacheKey.VideoCacheKey.ToString(), () => _cacheService.VideoCacheFillUp());
        result = _apiService.GetVideoForWebPage(f, offset, videoCache);
        _memoryCache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return result;
    }

    private TC GetCache<TC>(string key)
    {
        if (_memoryCache.TryGetValue(key, out var result) && result is TC cached)
        {
            return cached;
        }

        return default;
    }

    private TC GetCache<TC>(string key, Func<TC> function)
    {
        var cached = GetCache<TC>(key);
        if (cached is not null)
        {
            return cached;
        }

        var res = function();
        if (res is not null)
        {
            _memoryCache.Set(key, res, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
        }

        return res;
    }

    public string GetETag(int tipus, Guid id)
    {
        // Determine which cache key to use based on tipus
        string cacheKey;
        if (AndrokatConfiguration.BlogNewsContentTypeIds().Contains(tipus) || tipus == (int)Forras.book)
        {
            cacheKey = CacheKey.EgyebOlvasnivaloResponseCacheKey + "_" + tipus + "_" + id;
        }
        else
        {
            cacheKey = CacheKey.ContentResponseCacheKey + "_" + tipus + "_" + id;
        }

        var etagKey = cacheKey + "_etag";
        return GetCache<string>(etagKey);
    }
}