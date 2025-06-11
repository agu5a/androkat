using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace androkat.application.Service;

public class ContentService : IContentService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ICacheService _cacheService;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public ContentService(
        IMemoryCache memoryCache,
        ICacheService cacheService,
        IOptions<AndrokatConfiguration> androkatConfiguration)
    {
        _memoryCache = memoryCache;
        _cacheService = cacheService;
        _androkatConfiguration = androkatConfiguration;
    }

    public IReadOnlyCollection<ContentModel> GetHome()
    {
        var result = GetContentDetailsModel(
        [
            (int)Forras.advent,
            (int)Forras.maievangelium,
            (int)Forras.horvath,
            (int)Forras.nagybojt,
            (int)Forras.bojte,
            (int)Forras.barsi,
            (int)Forras.laciatya,
            (int)Forras.papaitwitter,
            (int)Forras.regnum,
            (int)Forras.fokolare,
            (int)Forras.prohaszka,
            (int)Forras.kempis,
            (int)Forras.taize,
            (int)Forras.szeretetujsag,
            (int)Forras.medjugorje,
            (int)Forras.mello
        ]);
        return result;
    }

    public IReadOnlyCollection<ContentModel> GetAjanlat()
    {
        var result = GetContentDetailsModel([(int)Forras.ajanlatweb]);
        return result;
    }

    public IReadOnlyCollection<ContentModel> GetSzentek()
    {
        var result = GetContentDetailsModel(
        [
            (int)Forras.vianney,
            (int)Forras.pio,
            (int)Forras.janospal,
            (int)Forras.szentbernat,
            (int)Forras.sztjanos,
            (int)Forras.kisterez,
            (int)Forras.terezanya,
            (int)Forras.ignac,
            (int)Forras.szentszalezi,
            (int)Forras.sienaikatalin
        ]);
        return result;
    }

    public IReadOnlyCollection<ContentModel> GetImaPage(string csoport)
    {
        var list = new List<ContentModel>();
        var result = GetIma(csoport);
        foreach (var item in result)
        {
            list.Add(new ContentModel(new ContentDetailsModel(item.Nid, DateTime.MinValue, item.Cim, string.Empty, default, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty)
            , default)
            );
        }

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<AudioModel> GetAudio()
    {
        var list = new List<AudioModel>();

        list.AddRange(GetAudioViewModel((int)Forras.audionapievangelium));
        list.AddRange(GetAudioViewModel((int)Forras.prayasyougo).OrderByDescending(o => o.Inserted).Take(2));
        list.AddRange(GetAudioViewModel((int)Forras.audiobarsi));
        list.AddRange(GetAudioViewModel((int)Forras.audiopalferi));
        list.AddRange(GetAudioViewModel((int)Forras.audiohorvath).OrderByDescending(o => o.Inserted).Take(2));
        list.AddRange(GetAudioViewModel((int)Forras.audiotaize));

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<VideoSourceModel> GetVideoSourcePage()
    {
        var list = new List<VideoSourceModel>();
        var result = GetVideoSource();
        foreach (var item in result)
        {
            list.Add(new VideoSourceModel(item.ChannelId, item.ChannelName));
        }

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<RadioModel> GetRadioPage()
    {
        var list = new List<RadioModel>();
        list.AddRange(GetRadio().Select(item => new RadioModel(item.Key, item.Value)));

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<ContentModel> GetSzent()
    {
        return GetContentDetailsModel([(int)Forras.maiszent]);
    }

    public IReadOnlyCollection<ContentModel> GetHirek(int tipus)
    {
        var list = new List<ContentModel>();
        var result = GetNewsByCategory(tipus);
        foreach (var item in result)
        {
            list.Add(new ContentModel(item, _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)));
        }

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<ContentModel> GetBlog(int tipus)
    {
        var list = new List<ContentModel>();
        var result = GetBlogByCategory(tipus);
        foreach (var item in result)
        {
            list.Add(new ContentModel(item, _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)));
        }
        return list.AsReadOnly();
    }

    public IReadOnlyCollection<ContentModel> GetHumor()
    {
        var list = new List<ContentModel>();
        var res = GetMainCache();

        foreach (var item in res.ContentDetailsModels.Where(w => w.Tipus == (int)Forras.humor))
        {
            list.Add(new ContentModel(item, _androkatConfiguration.Value.GetContentMetaDataModelByTipus((int)Forras.humor)));
        }

        return list.AsReadOnly();
    }

    public ContentModel GetContentDetailsModelByNid(Guid nid, int tipus)
    {
        var contentDetailsModel = GetMainCache().ContentDetailsModels.FirstOrDefault(w => w.Tipus == tipus && w.Nid == nid);
        if (contentDetailsModel is null)
        {
            return default;
        }

        var data = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus);
        return new ContentModel(contentDetailsModel, data);
    }

    public ImaModel GetImaById(Guid nid)
    {
        var res = GetCache(CacheKey.ImaCacheKey.ToString(), () => _cacheService.ImaCacheFillUp());
        return res.Imak.FirstOrDefault(w => w.Nid == nid);
    }

    private ReadOnlyCollection<ContentModel> GetContentDetailsModel(int[] tipusok)
    {
        var list = new List<ContentModel>();

        var result = Get(tipusok);
        foreach (var item in result)
        {
            list.Add(new ContentModel(item, _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)));
        }

        return list.OrderBy(o => o.MetaData.TipusId).ThenByDescending(o => o.ContentDetails.Fulldatum).ToList().AsReadOnly();
    }

    private IEnumerable<ContentDetailsModel> Get(int[] tipusok)
    {
        return GetMainCache().ContentDetailsModels.Where(w => tipusok.Contains(w.Tipus));
    }

    private static string GetAudio(string idezet)
    {
        var content = "<div></div>";
        content += "<div style=\"margin: 15px 0 0 0;\"><strong>Hangállomány meghallgatása</strong></div>";
        content += "<div style=\"margin: 0 0 15px 0;\"><audio controls><source src=\"" + idezet + "\" type=\"audio/mpeg\">Your browser does not support the audio element.</audio></div>";
        content += "<div style=\"margin: 0 0 15px 0;word-break: break-all;\"><strong>Vagy a letöltése</strong>: <a href=\"" + idezet + "\">" + idezet + "</a></div>";
        return content;
    }

    private List<AudioModel> GetAudioViewModel(int tipus)
    {
        var list = new List<AudioModel>();
        var contents = Get([tipus]);

        contents.ToList().ForEach(f =>
        {
            var url = string.IsNullOrEmpty(f.FileUrl) ? f.Idezet : f.FileUrl;
            var idezet = GetAudio(string.IsNullOrEmpty(f.FileUrl) ? f.Idezet : f.FileUrl);
            list.Add(new AudioModel(HttpUtility.UrlEncode(url), HttpUtility.UrlEncode(f.Cim), url, idezet, f.Inserted, f.Cim, tipus,
                _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus))
            );
        });

        return list;
    }

    private IEnumerable<ImaModel> GetIma(string csoport)
    {
        var res = GetCache(CacheKey.ImaCacheKey.ToString(), () => _cacheService.ImaCacheFillUp());

        return !string.IsNullOrWhiteSpace(csoport) ? res.Imak.Where(w => w.Csoport == csoport).OrderBy(o => o.Cim) : res.Imak.OrderBy(o => o.Cim);
    }

    private Dictionary<string, string> GetRadio()
    {
        var res = GetCache(CacheKey.BookRadioSysCacheKey.ToString(), () => _cacheService.BookRadioSysCacheFillUp());

        var ra = res.SystemData.FirstOrDefault(w => w.Key == "radio");
        return ra is null ? [] : JsonSerializer.Deserialize<Dictionary<string, string>>(ra.Value);
    }

    private IEnumerable<VideoSourceModel> GetVideoSource()
    {
        return GetCache(CacheKey.VideoCacheKey.ToString(), () => _cacheService.VideoCacheFillUp()).VideoSource;
    }

    private MainCache GetMainCache()
    {
        return GetCache(CacheKey.MainCacheKey.ToString(), () => _cacheService.MainCacheFillUp());
    }

    private IEnumerable<ContentDetailsModel> GetNewsByCategory(int tipus)
    {
        var res = GetCache(CacheKey.MainCacheKey.ToString(), () => _cacheService.MainCacheFillUp());

        if (tipus > 0)
        {
            return [.. res.Egyeb.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum)];
        }

        var list = new List<int> { (int)Forras.bonumtv, (int)Forras.kurir, (int)Forras.keresztenyelet };
        return res.Egyeb.Where(w => list.Contains(w.Tipus)).OrderByDescending(o => o.Fulldatum);
    }

    private IEnumerable<ContentDetailsModel> GetBlogByCategory(int tipus)
    {
        var res = GetCache(CacheKey.MainCacheKey.ToString(), () => _cacheService.MainCacheFillUp());

        if (tipus > 0)
        {
            return [.. res.Egyeb.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum)];
        }

        var list = new List<int> { (int)Forras.b777, (int)Forras.bzarandokma, (int)Forras.jezsuitablog };
        return res.Egyeb.Where(w => list.Contains(w.Tipus)).OrderByDescending(o => o.Fulldatum);
    }

    private C GetCache<C>(string key, Func<C> function)
    {
        if (_memoryCache.TryGetValue(key, out var result) && result is C cached)
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
}