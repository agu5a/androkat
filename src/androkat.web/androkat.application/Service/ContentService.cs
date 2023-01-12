using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
		var result = GetContentDetailsModel(new int[]
		{
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
			(int)Forras.szeretetujsag
		});
		return result;
	}

	public IReadOnlyCollection<ContentModel> GetAjanlat()
	{
		var result = GetContentDetailsModel(new int[] { (int)Forras.ajanlatweb });
		return result;
	}

	public IReadOnlyCollection<ContentModel> GetSzentek()
	{
		var result = GetContentDetailsModel(new int[]
		{
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
		});
		return result;
	}

	public IReadOnlyCollection<ContentModel> GetImaPage(string csoport)
	{
		var list = new List<ContentModel>();
		var result = GetIma(csoport);
		foreach (var item in result)
			list.Add(new ContentModel
			{
				ContentDetails = new ContentDetailsModel
				{
					Nid = item.Nid,
					Cim = item.Cim
				}
			});

		return list;
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

		return list;
	}

	public IReadOnlyCollection<VideoSourceModel> GetVideoSourcePage()
	{
		var list = new List<VideoSourceModel>();
		var result = GetVideoSource();
		foreach (var item in result)
			list.Add(new VideoSourceModel(item.ChannelId, item.ChannelName));

		return list;
	}

	public IReadOnlyCollection<RadioModel> GetRadioPage()
	{
		var list = new List<RadioModel>();
		list.AddRange(GetRadio().Select(item => new RadioModel(item.Key, item.Value)));

		return list;
	}

	public IReadOnlyCollection<ContentModel> GetSzent()
	{
		var result = GetContentDetailsModel(new int[] { (int)Forras.maiszent });
		return result;
	}

	public IReadOnlyCollection<ContentModel> GetHirek(int tipus)
	{
		var list = new List<ContentModel>();
		var result = GetNewsByCategory(tipus);
		foreach (var item in result)
		{
			list.Add(new ContentModel
			{
				ContentDetails = item,
				MetaData = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)
			});
		}

		return list;
	}

	public IReadOnlyCollection<ContentModel> GetBlog(int tipus)
	{
		var list = new List<ContentModel>();
		var result = GetBlogByCategory(tipus);
		foreach (var item in result)
		{
			list.Add(new ContentModel
			{
				ContentDetails = item,
				MetaData = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)
			});
		}
		return list;
	}

	public IReadOnlyCollection<ContentModel> GetHumor()
	{
		var list = new List<ContentModel>();
		var res = GetMainCache();

		foreach (var item in res.ContentDetailsModels.Where(w => w.Tipus == (int)Forras.humor))
			list.Add(new ContentModel { ContentDetails = item, MetaData = _androkatConfiguration.Value.GetContentMetaDataModelByTipus((int)Forras.humor) });

		return list;
	}

	public ContentModel GetContentDetailsModelByNid(Guid nid, int tipus)
	{
		var contentDetailsModel = GetMainCache().ContentDetailsModels.FirstOrDefault(w => w.Tipus == tipus && w.Nid == nid);
		if (contentDetailsModel == null)
			return default;

		var data = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus);
		return new ContentModel { ContentDetails = contentDetailsModel, MetaData = data };
	}

	public ImaModel GetImaById(Guid nid)
	{
		var res = GetCache<ImaCache>(CacheKey.ImaCacheKey.ToString(), () => { return _cacheService.ImaCacheFillUp(); });
		return res.Imak.FirstOrDefault(w => w.Nid == nid);
	}

	private IReadOnlyCollection<ContentModel> GetContentDetailsModel(int[] tipusok)
	{
		var list = new List<ContentModel>();

		var result = Get(tipusok);
		foreach (var item in result)
			list.Add(new ContentModel { ContentDetails = item, MetaData = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus) });

		return list;
	}

	private IEnumerable<ContentDetailsModel> Get(int[] tipusok)
	{
		var result = new List<ContentDetailsModel>();
		foreach (var item in GetMainCache().ContentDetailsModels.Where(w => tipusok.Contains(w.Tipus)))
		{
			result.Add(item);
		}

		return result;
	}

	private static string GetAudio(string idezet)
	{
		var content = "";
		var mp3 = GetMp3(idezet);

		if (idezet.Contains("[["))
		{
			var pieces = idezet.Split(new[] { "[[" }, System.StringSplitOptions.RemoveEmptyEntries);
			content += "<div>" + pieces[0] + "</div>";
		}
		else
		{
			content += "<div></div>";
		}

		content += "<div style=\"margin: 15px 0 0 0;\"><strong>Hangállomány meghallgatása</strong></div>";
		content += "<div style=\"margin: 0 0 15px 0;\"><audio controls><source src=\"" + mp3 + "\" type=\"audio/mpeg\">Your browser does not support the audio element.</audio></div>";
		content += "<div style=\"margin: 0 0 15px 0;word-break: break-all;\"><strong>Vagy a letöltése</strong>: <a href=\"" + mp3 + "\">" + mp3 + "</a></div>";
		return content;
	}

	private static string GetMp3(string idezet)
	{
		string mp3;
		if (idezet.Contains("[["))
		{
			var pieces = idezet.Split(new[] { "[[" }, StringSplitOptions.RemoveEmptyEntries);
			mp3 = pieces[1].Replace("]]", "");
		}
		else
		{
			mp3 = idezet;
		}

		return mp3;
	}

	private List<AudioModel> GetAudioViewModel(int tipus)
	{
		var list = new List<AudioModel>();
		var contents = Get(new int[] { tipus });

		contents.ToList().ForEach(f =>
		{
			var mp3 = GetMp3(string.IsNullOrEmpty(f.FileUrl) ? f.Idezet : f.FileUrl);
			var idezet = GetAudio(string.IsNullOrEmpty(f.FileUrl) ? f.Idezet : f.FileUrl);
			list.Add(new AudioModel
			{
				Cim = f.Cim,
				Inserted = f.Inserted,
				Tipus = tipus,
				Idezet = idezet,
				MetaDataModel = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus),
				//EncodedUrl = HttpUtility.UrlEncode(mp3),
				//ShareTitle = HttpUtility.UrlEncode(f.Cim),
				//Url = mp3                
			});
		});

		return list;
	}

	private IEnumerable<ImaModel> GetIma(string csoport)
	{
		var res = GetCache<ImaCache>(CacheKey.ImaCacheKey.ToString(), () => { return _cacheService.ImaCacheFillUp(); });

		List<ImaModel> result;

		if (!string.IsNullOrWhiteSpace(csoport))
			result = res.Imak.Where(w => w.Csoport == csoport).OrderBy(o => o.Cim).ToList();
		else
			result = res.Imak.OrderBy(o => o.Cim).ToList();

		return result;
	}

	private Dictionary<string, string> GetRadio()
	{
		var res = GetCache<BookRadioSysCache>(CacheKey.BookRadioSysCacheKey.ToString(), () => { return _cacheService.BookRadioSysCacheFillUp(); });

		var ra = res.SystemData.FirstOrDefault(w => w.Key == "radio");
		if (ra == null)
			return new Dictionary<string, string>();

		var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(ra.Value);
		if (dic == null)
			return new Dictionary<string, string>();

		return dic;
	}

	private IEnumerable<VideoSourceModel> GetVideoSource()
	{
		var res = GetCache<VideoCache>(CacheKey.VideoCacheKey.ToString(), () => { return _cacheService.VideoCacheFillUp(); });
		return res.VideoSource;
	}

	private MainCache GetMainCache()
	{
		var res = GetCache<MainCache>(CacheKey.MainCacheKey.ToString(), () => { return _cacheService.MainCacheFillUp(); });
		var mainCache = new MainCache
		{
			ContentDetailsModels = res.ContentDetailsModels
		};

		return mainCache;
	}

	private IEnumerable<ContentDetailsModel> GetNewsByCategory(int tipus)
	{
		var res = GetCache<EgyebCache>(CacheKey.EgyebCacheKey.ToString(), () => { return _cacheService.EgyebCacheFillUp(); });

		List<ContentDetailsModel> list;
		if (tipus > 0)
			{
			list = res.Egyeb.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum).ToList();
			}
		else
			list = res.Egyeb.Where(w => w.Tipus == (int)Forras.bonumtv || w.Tipus == (int)Forras.kurir || w.Tipus == (int)Forras.keresztenyelet).OrderByDescending(o => o.Fulldatum).ToList();

		return list;
	}

	private IEnumerable<ContentDetailsModel> GetBlogByCategory(int tipus)
	{
		var res = GetCache<EgyebCache>(CacheKey.EgyebCacheKey.ToString(), () => { return _cacheService.EgyebCacheFillUp(); });

		List<ContentDetailsModel> list;
		if (tipus > 0)
			{
			list = res.Egyeb.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum).ToList();
			}
		else
			list = res.Egyeb.Where(w => w.Tipus == (int)Forras.b777 || w.Tipus == (int)Forras.bkatolikusma || w.Tipus == (int)Forras.jezsuitablog).OrderByDescending(o => o.Fulldatum).ToList();

		return list;
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