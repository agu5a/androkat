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
        var res = Ctx.ImaContent.AsNoTracking().OrderBy(o => o.Datum);
        return Mapper.Map<IReadOnlyCollection<ImaModel>>(res);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetBooksToCache()
    {
        var res = Ctx.Content.AsNoTracking().Where(w => w.Tipus == 46).OrderByDescending(o => o.Inserted);
        return Mapper.Map<IReadOnlyCollection<ContentDetailsModel>>(res);
    }

    public IReadOnlyCollection<RadioMusorModel> GetRadioToCache()
    {
        var res = Ctx.RadioMusor.AsNoTracking();
        return Mapper.Map<IReadOnlyCollection<RadioMusorModel>>(res);
    }

    public IReadOnlyCollection<VideoSourceModel> GetVideoSourceToCache()
    {
        var res = Ctx.VideoContent.AsNoTracking().GroupBy(p => new { p.Forras, p.ChannelId })
                .Select(s => new VideoSource { ChannelName = s.Key.Forras, ChannelId = s.Key.ChannelId });
        return Mapper.Map<IReadOnlyCollection<VideoSourceModel>>(res);
    }

    public IReadOnlyCollection<VideoModel> GetVideoToCache()
    {
        var res = Ctx.VideoContent.AsNoTracking().OrderByDescending(o => o.Date).ThenByDescending(t => t.Inserted);
        return Mapper.Map<IReadOnlyCollection<VideoModel>>(res);
    }

    public IReadOnlyCollection<SystemInfoModel> GetSystemInfoToCache()
    {
        var res = Ctx.SystemInfo.AsNoTracking().Where(w => w.Key != "version");
        return Mapper.Map<IReadOnlyCollection<SystemInfoModel>>(res);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetHirekBlogokToCache()
    {
        var res = Ctx.Content.Where(w => AndrokatConfiguration.BlogNewsContentTypeIds().Contains(w.Tipus)).AsNoTracking()
            .OrderByDescending(o => o.Fulldatum);
        return Mapper.Map<IReadOnlyCollection<ContentDetailsModel>>(res);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetHumorToCache()
    {
        var list = new List<ContentDetailsModel>();

        var month = Clock.Now.ToString("MM");
        var tipus = (int)Forras.humor;
        var rows = Ctx.FixContent
            .Where(w => w.Tipus == tipus && w.Datum.StartsWith($"{month}-"))
            .OrderByDescending(o => o.Datum).ToList();
        
        rows = rows.Where(w => w.Fulldatum < Clock.Now.DateTime).ToList();

        rows.ForEach(w =>
        {
            list.Add(Mapper.Map<ContentDetailsModel>(w));
        });

        return Mapper.Map<IReadOnlyCollection<ContentDetailsModel>>(list);
    }

    public IReadOnlyCollection<ContentDetailsModel> GetMaiSzentToCache()
    {
        var list = new List<ContentDetailsModel>();
        var hoNap = Clock.Now.ToString("MM-dd");
        var month = Clock.Now.ToString("MM");

        var rows = Ctx.MaiSzent.AsNoTracking().Where(w => w.Datum == hoNap);
        if (!rows.Any())
        {
            _logger.LogDebug("{Name}: nincs mai szent mai napra", nameof(GetMaiSzentToCache));

            var rows2 = Ctx.MaiSzent.AsNoTracking().AsEnumerable()
            .Where(w => w.Datum.StartsWith($"{month}-") && w.Fulldatum < Clock.Now)
            .OrderByDescending(o => o.Datum).Take(1)!.ToList();

            if (rows2.Count == 0)
            {
                _logger.LogDebug("{Name}: nincs mai szent az aktuális hónapra", nameof(GetMaiSzentToCache));

                var prevmonth = Clock.Now.AddMonths(-1).ToString("MM");
                rows2 = Ctx.MaiSzent.AsNoTracking().AsEnumerable()
                    .Where(w => w.Datum.StartsWith($"{prevmonth}-"))
                    .OrderByDescending(o => o.Datum).Take(1).ToList();
            }

            rows2.ForEach(row =>
            {
                list.Add(Mapper.Map<ContentDetailsModel>(row));
            });
        }
        else
        {
            rows.ToList().ForEach(row =>
            {
                list.Add(Mapper.Map<ContentDetailsModel>(row));
            });
        }

        return list.AsReadOnly();
    }

    public IReadOnlyCollection<ContentDetailsModel> GetTodayFixContentToCache()
    {
        var result = new List<ContentDetailsModel>();

        var tipusok = new List<int>();
        AndrokatConfiguration.FixContentTypeIds().Where(w => w != (int)Forras.humor).ToList().ForEach(f =>
        {
            tipusok.Add(f);
        });

        var date = Clock.Now.ToString("MM-dd");

        var todayFixContents = Ctx.FixContent.AsNoTracking().Where(w => tipusok.Contains(w.Tipus) && w.Datum == date);
        foreach (var todayFixContent in todayFixContents)
        {
            result.Add(Mapper.Map<ContentDetailsModel>(todayFixContent));
        }

        return result.AsReadOnly();
    }

    public IReadOnlyCollection<ContentDetailsModel> GetContentDetailsModelToCache()
    {
        var result = new List<ContentDetailsModel>();

        var allRecords = Ctx.Content.AsNoTracking();

        //ezekből az összes elérhető kell, nem csak az adott napi
        var osszes = new List<int>
        {
            (int)Forras.audiohorvath, (int)Forras.audiotaize,
            (int)Forras.audiobarsi, (int)Forras.audionapievangelium,
            (int)Forras.ajanlatweb, (int)Forras.audiopalferi,
            (int)Forras.prayasyougo, (int)Forras.gyonas
        };

        var tomorrow = Clock.Now.AddDays(1).ToString("yyyy-MM-dd");

        foreach (var tipus in AndrokatConfiguration.ContentTypeIds())
        {
            var date = Clock.Now.ToString("yyyy-MM-dd");

            if (tipus == (int)Forras.fokolare)
            {
                date = Clock.Now.ToString("yyyy-MM") + "-01";
            }

            var res = GetContentDetailsModel(allRecords, tipus, date, tomorrow, osszes);
            result.AddRange(Mapper.Map<List<ContentDetailsModel>>(res));
        }

        var gyonas = GetContentDetailsModel(allRecords, (int)Forras.gyonas, Clock.Now.ToString("yyyy-MM-dd"), tomorrow, osszes);
        result.AddRange(Mapper.Map<List<ContentDetailsModel>>(gyonas));

        return result.AsReadOnly();
    }

    private List<Content> GetContentDetailsModel(IQueryable<Content> allRecords, int tipus, string date, string tomorrow, List<int> osszes)
    {
        List<Content> res;
        if (tipus == (int)Forras.maievangelium) //szombaton már megjelenik a vasárnapi is
        {
            res = [.. allRecords.Where(w => w.Tipus == tipus && (w.Fulldatum.StartsWith(date) || w.Fulldatum.StartsWith(tomorrow))).OrderByDescending(o => o.Inserted).ToList()];
        }
        else if (osszes.Contains(tipus)) //ajanlo és néhány hanganyagból a weboldalon látszik mindegyik 
        {
            res = [.. allRecords.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Inserted).ToList()];
        }
        else
        {
            res = [.. allRecords.Where(w => w.Tipus == tipus && w.Fulldatum.StartsWith(date)).OrderByDescending(o => o.Inserted).ToList()];
        }

        if (res!.Count != 0)
        {
            return res!;
        }

        _logger.LogDebug("{Name}: nincs mai, akkor egy a korábbiakból, ha van. tipus {Tipus} date {Date}", nameof(GetContentDetailsModel), tipus, date);
        res = allRecords.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Inserted).Take(1).ToList();

        return res!;
    }
}