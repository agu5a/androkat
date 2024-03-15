using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Service;

public class CronService : ICronService
{
    private readonly IApiRepository _apiRepository;
    private readonly IClock _clock;
    private readonly ILogger<CronService> _logger;
    private readonly IMemoryCache _memoryCache;

    public CronService(
        IApiRepository apiRepository,
        IClock iClock,
        ILogger<CronService> logger,
        IMemoryCache memoryCache)
    {
        _apiRepository = apiRepository;
        _clock = iClock;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public void Start()
    {
        try
        {
            DeleteOld((int)Forras.keresztenyelet, -3);
            DeleteOld((int)Forras.b777, -2);
            DeleteOld((int)Forras.bonumtv, -1);
            DeleteOld((int)Forras.bzarandokma, -2);
            DeleteOld((int)Forras.jezsuitablog, -3);
            DeleteOld((int)Forras.kurir, -1);
            DeleteOldRows((int)Forras.audiohorvath, 3);
            DeleteOldRows((int)Forras.fokolare, 1);
            DeleteOldRows((int)Forras.papaitwitter, 4);
            DeleteOldRows((int)Forras.advent, 2);
            DeleteOldRows((int)Forras.maievangelium, 3);
            DeleteOldRows((int)Forras.ignac, 3);
            DeleteOldRows((int)Forras.barsi, 3);
            DeleteOldRows((int)Forras.laciatya, 3);
            DeleteOldRows((int)Forras.horvath, 3);
            DeleteOldRows((int)Forras.prayasyougo, 7);
            DeleteOldRows((int)Forras.nagybojt, 2);
            DeleteOldRows((int)Forras.szeretetujsag, 5);
            DeleteOldRows((int)Forras.audionapievangelium, 2);
            DeleteOldRows((int)Forras.audiobarsi, 3);
            DeleteOldRows((int)Forras.audiopalferi, 3);
            DeleteOldRows((int)Forras.regnum, 3);
            DeleteOldRows((int)Forras.taize, 4);
            DeleteOldRows((int)Forras.szentbernat, 3);
            DeleteOldRows((int)Forras.audiotaize, 2);
            DeleteOldVideoRows(100);  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(Start));
        }
    }

    public void DeleteCaches()
    {
        try
        {
            if (_memoryCache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);//remove 100% cache content
                return;
            }

            throw new InvalidOperationException("Failed to run DeleteCaches");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(DeleteCaches));
        }
    }

    private void DeleteOld(int tipus, int days)
    {
        var osszes = _apiRepository.GetContentDetailsModels().Count(w => w.Tipus == tipus);
        if (osszes == 0)
            return;

        var mult = _clock.Now.DateTime.AddDays(days);
        var torlendo = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus && w.Fulldatum < mult).ToList();
        if (torlendo.Count == 0)
            return;

        if (osszes <= torlendo.Count)
            return;

        _logger.LogDebug("delete from Content. {Tipus} {Count}", tipus, torlendo.Count);
        foreach (var item in torlendo)
        {
            _apiRepository.DeleteContentDetailsByNid(item.Nid);
            _logger.LogDebug("delete from Content. {Tipus} {Time}", item.Tipus, item.Fulldatum);
        }
    }

    private void DeleteOldRows(int tipus, int max)
    {
        var osszes = new List<ContentDetailsModel>();

        if (_clock.Now.DateTime.ToString("MM") == "01") // törölni a tavalyit, egyébként a dátum sorrend miatt mindig az újakat törölné :((
            osszes = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus && !w.Fulldatum.ToString("MM-dd").StartsWith("01-")).Skip(max).ToList();

        if (osszes.Count == 0)
            osszes = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum).Skip(max).ToList();

        if (osszes!.Count == 0)
            return;

        foreach (var item in osszes!)
        {
            _apiRepository.DeleteContentDetailsByNid(item.Nid);
            _logger.LogDebug("delete from Content. {Tipus} {Datum}", item.Tipus, item.Fulldatum);
        }
    }

    private void DeleteOldVideoRows(int max)
    {
        var osszes = _apiRepository.GetVideoModels().OrderByDescending(o => o.Date).Skip(max).ToList();
        if (osszes.Count == 0)
            return;

        foreach (var item in osszes)
        {
            _apiRepository.DeleteVideoByNid(item.Nid);
            _logger.LogDebug("delete from videoContent. {Date}", item.Date);
        }
    }
}