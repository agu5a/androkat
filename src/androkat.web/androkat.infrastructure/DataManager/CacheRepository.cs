using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.DataManager;

public class CacheRepository : BaseRepository, ICacheRepository
{
    private readonly ILogger<CacheRepository> _logger;

    public CacheRepository(AndrokatContext ctx,
        ILogger<CacheRepository> logger,
        IClock clock,
        IMapper mapper) : base(ctx, clock, mapper)
    {
        _logger = logger;
    }

    public IReadOnlyCollection<ImaModel> GetImaToCache()
    {
        var res = _ctx.ImaContent.AsNoTracking().OrderBy(o => o.Datum);
        return _mapper.Map<IReadOnlyCollection<ImaModel>>(res);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetBooksToCache()
    {
        var res = _ctx.Content.AsNoTracking().Where(w => w.Tipus == 46).OrderByDescending(o => o.Inserted);
        return _mapper.Map<IReadOnlyCollection<ContentDetailsModel>>(res);
    }

    public IReadOnlyCollection<RadioMusorModel> GetRadioToCache()
    {
        var res = _ctx.RadioMusor.AsNoTracking();
        return _mapper.Map<IReadOnlyCollection<RadioMusorModel>>(res);
    }

    public IReadOnlyCollection<VideoSourceModel> GetVideoSourceToCache()
    {
        var res = _ctx.VideoContent.AsNoTracking().GroupBy(p => new { p.Forras, p.ChannelId })
                .Select(s => new VideoSource { ChannelName = s.Key.Forras, ChannelId = s.Key.ChannelId });
        return _mapper.Map<IReadOnlyCollection<VideoSourceModel>>(res);
    }

    public IReadOnlyCollection<VideoModel> GetVideoToCache()
    {
        var res = _ctx.VideoContent.AsNoTracking().OrderByDescending(o => o.Date).ThenByDescending(t => t.Inserted);
        return _mapper.Map<IReadOnlyCollection<VideoModel>>(res);
    }

    public IReadOnlyCollection<SystemInfoModel> GetSystemInfoToCache()
    {
        var res = _ctx.SystemInfo.AsNoTracking().Where(w => w.Key != "version");
        return _mapper.Map<IReadOnlyCollection<SystemInfoModel>>(res);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetHirekBlogokToCache()
    {
        var res = _ctx.Content.Where(w => AndrokatConfiguration.BlogNewsContentTypeIds().Contains(w.Tipus)).AsNoTracking()
            .OrderByDescending(o => o.Fulldatum);
        return _mapper.Map<IReadOnlyCollection<ContentDetailsModel>>(res);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetHumorToCache()
    {
        var list = new List<ContentDetailsModel>();

        var month = _clock.Now.ToString("MM");
        var rows = _ctx.FixContent.AsNoTracking().AsEnumerable()
            .Where(w => w.Tipus == (int)Forras.humor && w.Datum.StartsWith($"{month}-") && w.FullDate < _clock.Now)
            .OrderByDescending(o => o.Datum).ToList();

        rows.ForEach(w =>
        {
            list.Add(_mapper.Map<ContentDetailsModel>(w));
        });

        return _mapper.Map<IReadOnlyCollection<ContentDetailsModel>>(list);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetMaiSzentToCache()
    {
        var list = new List<ContentDetailsModel>();
        var hoNap = _clock.Now.ToString("MM-dd");
        var month = _clock.Now.ToString("MM");

        var rows = _ctx.MaiSzent.AsNoTracking().Where(w => w.Datum == hoNap);
        if (!rows.Any())
        {
			_logger.LogDebug("{name}: nincs mai szent mai napra", nameof(GetMaiSzentToCache));

            var rows2 = _ctx.MaiSzent.AsNoTracking().AsEnumerable()
            .Where(w => w.Datum.StartsWith($"{month}-") && w.FullDate < _clock.Now)
            .OrderByDescending(o => o.Datum).Take(1);

            if (!rows2.Any())
            {
				_logger.LogDebug("{name}: nincs mai szent az aktuális hónapra", nameof(GetMaiSzentToCache));

                var prevmonth = _clock.Now.AddMonths(-1).ToString("MM");
                rows2 = _ctx.MaiSzent.AsNoTracking().AsEnumerable()
					.Where(w => w.Datum.StartsWith($"{prevmonth}-"))
                    .OrderByDescending(o => o.Datum).Take(1);
            }

            rows2.ToList().ForEach(row =>
            {
                list.Add(_mapper.Map<ContentDetailsModel>(row));
            });
        }
        else
            rows.ToList().ForEach(row =>
            {
                list.Add(_mapper.Map<ContentDetailsModel>(row));
            });

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<ContentDetailsModel> GetNapiFixToCache()
    {
        var result = new List<ContentDetailsModel>();

        var tipusok = new List<int>();
        AndrokatConfiguration.FixContentTypeIds().Where(w => w != (int)Forras.humor).ToList().ForEach(f =>
        {
            tipusok.Add(f);
        });

        var date = _clock.Now.ToString("MM-dd");

        var napiFixek = _ctx.FixContent.AsNoTracking().Where(w => tipusok.Contains(w.Tipus) && w.Datum == date);
            foreach (var napiFix in napiFixek)
            {
                result.Add(_mapper.Map<ContentDetailsModel>(napiFix));
            }

        return result.AsReadOnly();
    }

    public IReadOnlyCollection<ContentDetailsModel> GetContentDetailsModelToCache()
    {
        var result = new List<ContentDetailsModel>();

        var allRecords = _ctx.Content.AsNoTracking().ToList();

        //ezekből az összes elérhető kell, nem csak az adott napi
        var osszes = new List<int>
        {
            (int)Forras.audiohorvath, (int)Forras.audiotaize,
            (int)Forras.audiobarsi, (int)Forras.audionapievangelium,
            (int)Forras.ajanlatweb, (int)Forras.audiopalferi,
            (int)Forras.prayasyougo
        };

        var tomorrow = _clock.Now.AddDays(1).ToString("yyyy-MM-dd");

        foreach (var tipus in AndrokatConfiguration.ContentTypeIds())
        {
            var date = _clock.Now.ToString("yyyy-MM-dd");

            if (tipus == (int)Forras.fokolare)
                date = _clock.Now.ToString("yyyy-MM") + "-01";

			var res = GetContentDetailsModel(allRecords, tipus, date, tomorrow, osszes);
            result.AddRange(_mapper.Map<List<ContentDetailsModel>>(res));
        }

        return result.AsReadOnly();
    }

	private IEnumerable<Content> GetContentDetailsModel(List<Content> allRecords, int tipus, string date, string tomorrow, List<int> osszes)
    {
        IEnumerable<Content> res;
        if (tipus == (int)Forras.maievangelium) //szombaton már megjelenik a vasárnapi is
            res = allRecords.Where(w => w.Tipus == tipus && (w.Fulldatum.StartsWith(date) || w.Fulldatum.StartsWith(tomorrow))).OrderByDescending(o => o.Inserted);
        else if (osszes.Contains(tipus)) //ajanlo és néhány hanganyagból a weboldalon látszik mindegyik 
            res = allRecords.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Inserted);
        else
            res = allRecords.Where(w => w.Tipus == tipus && w.Fulldatum.StartsWith(date)).OrderByDescending(o => o.Inserted);

        if (res is null || !res.Any())
		{
			_logger.LogDebug("{name}: nincs mai, akkor egy a korábbiakból, ha van. tipus {tipus} date {date}", nameof(GetContentDetailsModel), tipus, date);
            res = allRecords.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Inserted).Take(1);
		}

        return res;
    }    
}