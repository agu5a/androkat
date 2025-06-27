using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace androkat.application.Service;

public class CronService : ICronService
{
    private readonly IApiRepository _apiRepository;
    private readonly IClock _clock;
    private readonly ILogger<CronService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IAdminRepository _adminRepository;

    public CronService(
        IApiRepository apiRepository,
        IClock iClock,
        ILogger<CronService> logger,
        IMemoryCache memoryCache,
        IAdminRepository adminRepository)
    {
        _apiRepository = apiRepository;
        _clock = iClock;
        _logger = logger;
        _memoryCache = memoryCache;
        _adminRepository = adminRepository;
    }

    public int Start()
    {
        try
        {
            var totalDeleted = 0;
            totalDeleted += DeleteOld((int)Forras.keresztenyelet, -3);
            totalDeleted += DeleteOld((int)Forras.b777, -2);
            totalDeleted += DeleteOld((int)Forras.bonumtv, -1);
            totalDeleted += DeleteOld((int)Forras.bzarandokma, -2);
            totalDeleted += DeleteOld((int)Forras.jezsuitablog, -3);
            totalDeleted += DeleteOld((int)Forras.kurir, -1);
            totalDeleted += DeleteOldRows((int)Forras.audiohorvath, 3);
            totalDeleted += DeleteOldRows((int)Forras.fokolare, 1);
            totalDeleted += DeleteOldRows((int)Forras.papaitwitter, 3);
            totalDeleted += DeleteOldRows((int)Forras.advent, 2);
            totalDeleted += DeleteOldRows((int)Forras.maievangelium, 3);
            totalDeleted += DeleteOldRows((int)Forras.ignac, 3);
            totalDeleted += DeleteOldRows((int)Forras.barsi, 3);
            totalDeleted += DeleteOldRows((int)Forras.laciatya, 3);
            totalDeleted += DeleteOldRows((int)Forras.horvath, 3);
            totalDeleted += DeleteOldRows((int)Forras.prayasyougo, 7);
            totalDeleted += DeleteOldRows((int)Forras.nagybojt, 2);
            totalDeleted += DeleteOldRows((int)Forras.szeretetujsag, 3);
            totalDeleted += DeleteOldRows((int)Forras.audionapievangelium, 2);
            totalDeleted += DeleteOldRows((int)Forras.audiobarsi, 3);
            totalDeleted += DeleteOldRows((int)Forras.audiopalferi, 3);
            totalDeleted += DeleteOldRows((int)Forras.regnum, 3);
            totalDeleted += DeleteOldRows((int)Forras.taize, 4);
            totalDeleted += DeleteOldRows((int)Forras.szentbernat, 3);
            totalDeleted += DeleteOldRows((int)Forras.audiotaize, 2);
            DeleteOldVideoRows(100);
            return totalDeleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(Start));
            return 0;
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

    private int DeleteOld(int tipus, int days)
    {
        var osszes = _apiRepository.GetContentDetailsModels().Count(w => w.Tipus == tipus);
        if (osszes == 0)
        {
            return 0;
        }

        var mult = _clock.Now.DateTime.AddDays(days);
        var torlendo = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus && w.Fulldatum < mult);
        if (!torlendo.Any())
        {
            return 0;
        }

        if (osszes <= torlendo.Count())
        {
            return 0;
        }

        var deletedCount = 0;
        _logger.LogDebug("delete from Content. {Tipus} {Count}", tipus, torlendo.Count());
        foreach (var item in torlendo)
        {
            _apiRepository.DeleteContentDetailsByNid(item.Nid);
            _logger.LogDebug("delete from Content. {Tipus} {Time}", item.Tipus, item.Fulldatum);
            deletedCount++;
        }
        return deletedCount;
    }

    private int DeleteOldRows(int tipus, int max)
    {
        IEnumerable<ContentDetailsModel> osszes;

        if (_clock.Now.DateTime.ToString("MM") == "01") // törölni a tavalyit ha január hónapban vagyunk, egyébként a dátum sorrend miatt mindig az újakat törölné :((
        {
            osszes = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus && !w.Fulldatum.ToString("MM-dd").StartsWith("01-")).Skip(max);
        }
        else
        {
            osszes = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum).Skip(max);
        }

        if (!osszes.Any())
        {
            return 0;
        }

        var deletedCount = 0;
        foreach (var item in osszes)
        {
            _apiRepository.DeleteContentDetailsByNid(item.Nid);
            _logger.LogDebug("delete from Content. {Tipus} {Datum}", item.Tipus, item.Fulldatum);
            deletedCount++;
        }
        return deletedCount;
    }

    private void DeleteOldVideoRows(int max)
    {
        var osszes = _apiRepository.GetVideoModels().OrderByDescending(o => o.Date).Skip(max);
        if (!osszes.Any())
        {
            return;
        }

        foreach (var item in osszes)
        {
            _apiRepository.DeleteVideoByNid(item.Nid);
            _logger.LogDebug("delete from videoContent. {Date}", item.Date);
        }
    }

    public List<string> DeleteFiles(string webRootPath, bool shouldDelete = false)
    {
        var res = _adminRepository.GetImgList();
        var filePath = Path.Combine(webRootPath, "images/ajanlatok");
        var files = Directory.GetFiles(filePath).OrderBy(o => o).ToList();
        List<string> fileNames = [];

        if (files.Count != 0)
        {
            files.ForEach(f =>
            {
                var fileName = f.Replace(filePath, string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty);
                var imgData = res.FirstOrDefault(i => i.Img == fileName);
                if (imgData is null && File.GetLastWriteTime(f) < DateTime.Now.AddDays(-20))
                {
                    fileNames.Add(f);
                }
            });
        }

        var filePaths = _adminRepository.GetAudioList();
        filePath = Path.Combine(webRootPath, "download");
        files = [.. Directory.GetFiles(filePath)];

        if (files.Count != 0)
        {
            files.ForEach(f =>
            {
                var fileName = f.Replace(filePath, string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty);
                var fileData = filePaths.FirstOrDefault(d => d.Path.Contains(fileName));
                if (fileData is null && File.GetLastWriteTime(f) < DateTime.Now.AddDays(-20))
                {
                    fileNames.Add(f);
                }
            });
        }

        if (!shouldDelete)
        {
            return fileNames;
        }

        foreach (var file in fileNames.Where(file => !file.Contains(".gitignore")))
        {
            File.Delete(file);
        }

        return fileNames;
    }
}