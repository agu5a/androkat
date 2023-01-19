using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace androkat.application.Service;

public class ApiService : IApiService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IClock _iClock;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public ApiService(ICacheService cacheService, IMemoryCache memoryCache, IClock iClock, IMapper mapper)
    {
        _memoryCache = memoryCache;
        _iClock = iClock;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset)
    {
        string key = CacheKey.VideoResponseCacheKey.ToString() + "_" + offset;
        var result = GetCache(key, () =>
        {
            var res = GetCache(CacheKey.VideoCacheKey.ToString(), () => { return _cacheService.VideoCacheFillUp(); });
            var temp = new List<VideoResponse>();
            foreach (var item in res.Video.OrderByDescending(o => o.Date).Skip(offset).Take(5))
            {
                temp.Add(new VideoResponse
                {
                    ChannelId = item.ChannelId,
                    Cim = item.Cim,
                    Date = item.Date,
                    Forras = item.Forras,
                    Img = item.Img,
                    VideoLink = item.VideoLink
                });
            }

            return temp;
        });

        return result.AsReadOnly();
    }

    private C GetCache<C>(string key, Func<C> function)
    {
        if (_memoryCache.TryGetValue(key, out var result) && result is C cached)
            return cached;

        var res = function();
        if (res != null)
            _memoryCache.Set(key, res, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return res;
    }
}