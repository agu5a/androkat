using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.domain.Model.WebResponse;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace androkat.infrastructure.DataManager;

public class AdminRepository : BaseRepository, IAdminRepository
{
    private readonly ILogger<AdminRepository> _logger;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public AdminRepository(
        AndrokatContext ctx,
        ILogger<AdminRepository> logger,
        IClock clock, IOptions<AndrokatConfiguration> androkatConfiguration,
        IMapper mapper) : base(ctx, clock, mapper)
    {
        _logger = logger;
        _androkatConfiguration = androkatConfiguration;
    }

    public bool DeleteTempContentByNid(string nid)
    {
        _logger.LogDebug("{Method} was called, {nid}", nameof(DeleteTempContentByNid), nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = _ctx.TempContent.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                _ctx.TempContent.Remove(res);
                _ctx.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool InsertContent(ContentDetailsModel content)
    {
        _logger.LogDebug("InsertNapiOlvaso was called, {cim} {tipus}", content.Cim, content.Tipus);

        try
        {
            var temp = new ContentDetailsModel(Guid.NewGuid(), content.Fulldatum, content.Cim, content.Idezet.Replace("\n", " ").Replace("\r", " "), content.Tipus,
                content.Inserted, content.KulsoLink, content.Img, content.FileUrl, content.Forras);
            _ctx.Content.Add(_mapper.Map<Content>(temp));
            _ctx.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
        return false;
    }

    public IEnumerable<AllTodayResult> LoadAllTodayResult()
    {
        _logger.LogDebug("{Method} was called", nameof(LoadAllTodayResult));
        var temp = new List<AllTodayResult>();

        try
        {
            temp.AddRange(_ctx.TempContent.Select(s => new AllTodayResult
            {
                Tipus = s.Tipus,
                TipusNev = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(s.Tipus).TipusNev,
                Datum = s.Fulldatum,
                Nid = s.Nid.ToString()
            }));

            foreach (var tipus in AndrokatConfiguration.ContentTypeIds())
            {
                if (!temp.Exists(c => c.Tipus == tipus))
                {
                    temp.Add(new AllTodayResult
                    {
                        Tipus = tipus,
                        Datum = string.Empty,
                        Nid = string.Empty,
                        TipusNev = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus).TipusNev
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return temp.OrderBy(o => o.TipusNev).ToList();
    }

    public ContentResult LoadPufferNapiByNid(string nid)
    {
        _logger.LogDebug("{method} was called, {nid}", nameof(LoadPufferNapiByNid), nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = _ctx.TempContent.FirstOrDefault(g => g.Nid == guid);

            if (res is not null)
                return new ContentResult
                {
                    Cim = res.Cim,
                    FullDatum = res.Fulldatum,
                    Forras = res.Forras,
                    Idezet = res.Idezet,
                    Img = res.Img,
                    Nid = res.Nid.ToString(),
                    Def = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(res.Tipus).TipusNev,
                    FileUrl = res.FileUrl,
                    Tipus = res.Tipus
                };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return null;
    }

    public IEnumerable<ImgData> GetImgList()
    {
        _logger.LogDebug("GetImgList was called");

        try
        {
            var res = _ctx.Content.Where(g => g.Img != "" && g.Img != null)
                .Select(s => new ImgData { Img = s.Img, Cim = s.Cim, Tipus = ((Forras)s.Tipus).ToString() }).ToList();

            if (res is null)
                return Enumerable.Empty<ImgData>();

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return Enumerable.Empty<ImgData>();
    }

    public IEnumerable<FileData> GetAudioList()
    {
        _logger.LogDebug("GetAudioList was called");

        try
        {
            var res = _ctx.Content.Where(g => g.FileUrl != "" && g.FileUrl != null)
                .Select(s => new FileData { Path = s.FileUrl, Cim = s.Cim, Tipus = ((Forras)s.Tipus).ToString() }).ToList();

            if (res is null)
                return Enumerable.Empty<FileData>();

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return Enumerable.Empty<FileData>();
    }

    public LastTodayResult GetLastTodayContentByTipus(int tipus)
    {
        _logger.LogDebug("{Method} was called, {tipus}", nameof(GetLastTodayContentByTipus), tipus);

        try
        {
            var date = _clock.Now.ToString("yyyy-MM-dd");
            var hoNap = _clock.Now.ToString("MM-dd");
            if (tipus == 7)
            {
                date = _clock.Now.ToString("yyyy-MM-01");
                hoNap = _clock.Now.ToString("MM-01");
            }

            ContentDetailsModel res = null;

            if (!AndrokatConfiguration.FixContentTypeIds().Contains(tipus))
            {
                var firstByDateAndTipus = _ctx.Content.FirstOrDefault(g => g.Tipus == tipus && g.Fulldatum.StartsWith(date));
                if (firstByDateAndTipus is not null)
                    res = _mapper.Map<ContentDetailsModel>(firstByDateAndTipus);
            }
            else
            {
                var fix = _ctx.FixContent.FirstOrDefault(g => g.Tipus == tipus && g.Datum == hoNap);
                if (fix is not null)
                    res = _mapper.Map<ContentDetailsModel>(fix);
            }

            if (res is null)
            {
                var firstByTipus = _ctx.Content.Where(g => g.Tipus == tipus).OrderByDescending(o => o.Fulldatum).FirstOrDefault();
                if (firstByTipus is not null)
                    res = _mapper.Map<ContentDetailsModel>(firstByTipus);
            }

            if (res is null)
                return new LastTodayResult();

            var meta = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus);

            return new LastTodayResult
            {
                Cim = res.Cim,
                Idezet = res.Idezet,
                FileUrl = res.FileUrl,
                Def = meta.TipusNev,
                Link = meta.Link,
                FullDatum = res.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss"),
                Segedlink = meta.Segedlink
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return new LastTodayResult();
    }
}