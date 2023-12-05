using androkat.domain.Enum;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.ContentCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "<Pending>")]
    private List<AllCachedResult> GetCache()
    {
        var res = new List<AllCachedResult>();

        try
        {
            var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            var coherentStateValue = coherentState.GetValue(_memoryCache);
            var entriesCollection = coherentStateValue.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            if (entriesCollection.GetValue(coherentStateValue) is not ICollection entriesCollectionValue)
            {
                return res;
            }

            foreach (var item in entriesCollectionValue)
            {
                var methodInfo = item.GetType().GetProperty("Key");

                var val = methodInfo.GetValue(item).ToString();

                if (val == CacheKey.MainCacheKey.ToString())
                {
                    _ = _memoryCache.TryGetValue(CacheKey.MainCacheKey.ToString(), out var result);
                    res.Add(new AllCachedResult { Key = CacheKey.MainCacheKey.ToString(), Inserted = ((MainCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else if (val == CacheKey.BookRadioSysCacheKey.ToString())
                {
                    _ = _memoryCache.TryGetValue(CacheKey.BookRadioSysCacheKey.ToString(), out var result);
                    res.Add(new AllCachedResult { Key = CacheKey.BookRadioSysCacheKey.ToString(), Inserted = ((BookRadioSysCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else if (val == CacheKey.ImaCacheKey.ToString())
                {
                    _ = _memoryCache.TryGetValue(CacheKey.ImaCacheKey.ToString(), out var result);
                    res.Add(new AllCachedResult { Key = CacheKey.ImaCacheKey.ToString(), Inserted = ((ImaCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else if (val == CacheKey.VideoCacheKey.ToString())
                {
                    _ = _memoryCache.TryGetValue(CacheKey.VideoCacheKey.ToString(), out var result);
                res.Add(new AllCachedResult { Key = CacheKey.VideoCacheKey.ToString(), Inserted = ((VideoCache)result).Inserted.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    res.Add(new AllCachedResult { Key = val.ToString() });
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return [.. res.OrderBy(o => o.Key)];
    }
}