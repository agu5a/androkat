using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace androkat.application.Service;

public partial class CacheService : ICacheService
{
    private readonly ICacheRepository _cacheRepository;
    private readonly ILogger<CacheService> _logger;
    private readonly IClock _clock;

    [GeneratedRegex("https:\\/\\/www.youtube.com\\/embed\\/([A-Za-z0-9-_]*)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex YoutubeRegex();

    public CacheService(ICacheRepository cacheRepository, ILogger<CacheService> logger, IClock clock)
    {
        _cacheRepository = cacheRepository;
        _logger = logger;
        _clock = clock;
    }

    public VideoCache VideoCacheFillUp()
    {
        _logger.LogInformation("{name} was called", nameof(VideoCacheFillUp));

        try
        {
            var res = _cacheRepository.GetVideoSourceToCache();

            var videoModel = new List<VideoModel>();
            var videok = _cacheRepository.GetVideoToCache();
            foreach (var item in videok)
            {
                var link = item.VideoLink;

                if (link.Contains("embed"))
                {
                    var match = YoutubeRegex().Match(item.VideoLink);
                    if (match.Success)
                        link = "https://www.youtube.com/watch?v=" + match.Groups[1].Value;
                }
                videoModel.Add(new VideoModel(item.Nid, item.Img, link, item.Cim, item.Date, item.Forras, item.ChannelId, item.Inserted));
            }

            return new VideoCache
            {
                VideoSource = res,
                Video = videoModel.AsReadOnly(),
                Inserted = _clock.Now.DateTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: {name}", nameof(VideoCacheFillUp));
        }

        return new VideoCache
        {
            VideoSource = new List<VideoSourceModel>(),
            Video = new List<VideoModel>(),
            Inserted = _clock.Now.DateTime
        };
    }

    public BookRadioSysCache BookRadioSysCacheFillUp()
    {
        _logger.LogInformation("{name} was called", nameof(BookRadioSysCacheFillUp));

        try
        {
            return new BookRadioSysCache
            {
                Books = _cacheRepository.GetBooksToCache(),
                RadioMusor = _cacheRepository.GetRadioToCache(),
                SystemData = _cacheRepository.GetSystemInfoToCache(),
                Inserted = _clock.Now.DateTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: {name}", nameof(BookRadioSysCacheFillUp));
        }

        return new BookRadioSysCache
        {
            Books = new List<ContentDetailsModel>(),
            RadioMusor = new List<RadioMusorModel>(),
            SystemData = new List<SystemInfoModel>(),
            Inserted = _clock.Now.DateTime
        };
    }

    public ImaCache ImaCacheFillUp()
    {
        _logger.LogInformation("{name} was called", nameof(ImaCacheFillUp));

        try
        {
            return new ImaCache
            {
                Imak = _cacheRepository.GetImaToCache(),
                Inserted = _clock.Now.DateTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: {name}", nameof(ImaCacheFillUp));
        }

        return new ImaCache
        {
            Imak = new List<ImaModel>(),
            Inserted = _clock.Now.DateTime
        };
    }

    public MainCache MainCacheFillUp()
    {
        _logger.LogInformation("{name} was called", nameof(MainCacheFillUp));

        try
        {
            var result = new List<ContentDetailsModel>();
            result.AddRange(_cacheRepository.GetHumorToCache());
            result.AddRange(_cacheRepository.GetMaiSzentToCache());
            result.AddRange(_cacheRepository.GetTodayFixContentToCache());
            result.AddRange(_cacheRepository.GetContentDetailsModelToCache());

            return new MainCache
            {
                ContentDetailsModels = result,
                Egyeb = _cacheRepository.GetHirekBlogokToCache(),
                Inserted = _clock.Now.DateTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: {name}", nameof(MainCacheFillUp));
        }

        return new MainCache
        {
            ContentDetailsModels = new List<ContentDetailsModel>(),
            Egyeb = new List<ContentDetailsModel>(),
            Inserted = _clock.Now.DateTime
        };
    }
}