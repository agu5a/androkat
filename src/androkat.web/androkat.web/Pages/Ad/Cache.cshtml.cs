using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.ContentCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Pages.Ad;

//[Authorize()]
public class CacheModel : PageModel
{
    private readonly IMemoryCache _memoryCache;
    protected readonly ILogger<CacheModel> _logger;

    public CacheModel(IMemoryCache memoryCache, ILogger<CacheModel> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public List<AllCachedResult> AllCachedResult { get; set; }
    public long Torolve { get; set; }
    public long CacheSize { get; set; }

    public void OnGet()
    {
        AllCachedResult = GetCache();
    }

    public void OnPostAll()
    {
        try
        {
            Torolve = _memoryCache.GetCurrentStatistics()?.CurrentEntryCount ?? 0L;
            CacheSize = _memoryCache.GetCurrentStatistics()?.CurrentEstimatedSize ?? 0L;

            if (_memoryCache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);//remove 100% cache content
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        AllCachedResult = GetCache();
    }

    private List<AllCachedResult> GetCache()
    {
        var res = new List<AllCachedResult>();
        try
        {
            if (_memoryCache.TryGetValue(CacheKey.MainCacheKey.ToString(), out var result))
                res.Add(new AllCachedResult { Key = CacheKey.MainCacheKey.ToString(), Inserted = ((MainCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });

            if (_memoryCache.TryGetValue(CacheKey.BookRadioSysCacheKey.ToString(), out result))
                res.Add(new AllCachedResult { Key = CacheKey.BookRadioSysCacheKey.ToString(), Inserted = ((BookRadioSysCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });

            if (_memoryCache.TryGetValue(CacheKey.ImaCacheKey.ToString(), out result))
                res.Add(new AllCachedResult { Key = CacheKey.ImaCacheKey.ToString(), Inserted = ((ImaCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });

            if (_memoryCache.TryGetValue(CacheKey.VideoCacheKey.ToString(), out result))
                res.Add(new AllCachedResult { Key = CacheKey.VideoCacheKey.ToString(), Inserted = ((VideoCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return [.. res.OrderBy(o => o.Key)];
    }
}