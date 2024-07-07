#nullable enable
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

    public List<AllUserResult> GetUsers()
    {
        var items = Ctx.Admin.ToList();
        return items.Select(item => new AllUserResult { Email = item.Email, LastLogin = item.LastLogin.ToString("yyyy-MM-dd HH:mm:ss") }).ToList();
    }

    public bool LogInUser(string email)
    {
        _logger.LogDebug("LogInUser was called, email: {Email}", email);
        try
        {
            var res = Ctx.Admin.FirstOrDefault(f => f.Email == email);
            if (res is not null)
            {
                res.LastLogin = Clock.Now.DateTime;
                Ctx.SaveChanges();
            }
            else
            {
                Ctx.Admin.Add(new Admin { Email = email, LastLogin = Clock.Now.DateTime });
                Ctx.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: LogInUser is failed {Email}", email);
        }
        return false;
    }

    public bool DeleteRadio(string nid)
    {
        _logger.LogDebug("DeleteRadio was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.RadioMusor.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                Ctx.RadioMusor.Remove(res);
                Ctx.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool DeleteTempContentByNid(string nid)
    {
        _logger.LogDebug("{Method} was called, {Nid}", nameof(DeleteTempContentByNid), nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.TempContent.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                Ctx.TempContent.Remove(res);
                Ctx.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool DeleteIma(string nid)
    {
        _logger.LogDebug("DeleteIma was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.ImaContent.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                Ctx.ImaContent.Remove(res);
                Ctx.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool DeleteContent(string nid)
    {
        _logger.LogDebug("DeleteContent was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.Content.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                Ctx.Content.Remove(res);
                Ctx.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public (bool isSuccess, string? message) InsertIma(ImaModel imaModel)
    {
        _logger.LogDebug("InsertIma was called, {Nid}", imaModel.Nid);

        try
        {
            Ctx.ImaContent.Add(Mapper.Map<ImaContent>(imaModel));
            Ctx.SaveChanges();
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            return (false, ex.Message);
        }
    }

    public (bool isSuccess, string? message) InsertFixContent(string cim, string idezet, int tipus, string datum)
    {
        _logger.LogDebug("InsertFixContent was called, {Cim} {Tipus}", cim, tipus);

        try
        {
            var res = Ctx.FixContent.FirstOrDefault(f => f.Datum == datum && f.Tipus == tipus);
            if (res is not null)
                return (false, "the record already exists with the same date and tipus");

            Ctx.FixContent.Add(new FixContent
            {
                Cim = cim.Replace("\n", " ").Replace("\r", " "),
                Idezet = idezet.Replace("\n", " ").Replace("\r", " "),
                Tipus = tipus,
                Datum = datum,
                Nid = Guid.NewGuid()
            });
            Ctx.SaveChanges();
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            return (false, ex.Message);
        }
    }

    public bool InsertContent(ContentDetailsModel content)
    {
        _logger.LogDebug("InsertContent was called, {Cim} {Tipus}", content.Cim, content.Tipus);

        try
        {
            var temp = new ContentDetailsModel(Guid.NewGuid(), content.Fulldatum, content.Cim, content.Idezet.Replace("\n", " ").Replace("\r", " "), content.Tipus,
                content.Inserted, content.KulsoLink, content.Img, content.FileUrl, content.Forras);
            Ctx.Content.Add(Mapper.Map<Content>(temp));
            Ctx.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
        return false;
    }

    public bool InsertError(ErrorRequest content)
    {
        _logger.LogWarning("InsertError was called: {Error}", content.Error);

        try
        {
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
        return false;
    }

    public bool UpdateContent(ContentDetailsModel content)
    {
        _logger.LogDebug("UpdateContent was called, {Nid}", content.Nid);

        try
        {
            var idezet = content.Idezet.Replace("\n", " ").Replace("\r", " ");

            var res = Ctx.Content.FirstOrDefault(f => f.Nid == content.Nid);
            if (res is null)
                return false;

            res.Idezet = idezet;
            res.FileUrl = content.FileUrl;
            res.Img = content.Img;
            res.Cim = content.Cim;
            res.Forras = content.Forras;
            res.Fulldatum = content.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss");
            res.Inserted = content.Inserted;
            Ctx.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool UpdateMaiSzent(MaiSzentModel maiszent)
    {
        _logger.LogDebug("UpdateMaiSzent was called, {Nid}", maiszent.Nid);

        try
        {
            maiszent.Idezet = maiszent.Idezet.Replace("\n", " ").Replace("\r", " ");

            var res = Ctx.MaiSzent.FirstOrDefault(f => f.Nid == maiszent.Nid);
            if (res is null)
                return false;

            res.Idezet = maiszent.Idezet;
            res.Img = maiszent.Img;
            res.Cim = maiszent.Cim;
            res.Datum = maiszent.Datum;
            res.Inserted = maiszent.Inserted;
            Ctx.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool UpdateIma(ImaModel imaModel)
    {
        _logger.LogDebug("UpdateIma was called, {Nid}", imaModel.Nid);

        try
        {
            var szoveg = imaModel.Szoveg.Replace("\n", " ").Replace("\r", " ");

            var res = Ctx.ImaContent.FirstOrDefault(f => f.Nid == imaModel.Nid);
            if (res is null)
                return false;

            res.Szoveg = szoveg;
            res.Cim = imaModel.Cim;
            res.Datum = imaModel.Datum;
            res.Csoport = imaModel.Csoport;
            Ctx.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool UpdateRadio(RadioMusorModel radioMusorModel)
    {
        _logger.LogDebug("UpdateRadio was called, {Nid}", radioMusorModel.Nid);

        try
        {
            var res = Ctx.RadioMusor.FirstOrDefault(f => f.Nid == radioMusorModel.Nid);
            if (res is null)
                return false;

            res.Musor = radioMusorModel.Musor;
            res.Inserted = radioMusorModel.Inserted;
            Ctx.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return false;
    }

    public bool UpdateSystemInfo(SystemInfoData systemInfoData)
    {
        _logger.LogDebug("UpdateSystemInfo was called, {Nid}", systemInfoData.Id);

        try
        {
            var res = Ctx.SystemInfo.FirstOrDefault(f => f.Id == systemInfoData.Id);
            if (res is null)
                return false;

            res.Value = systemInfoData.Value;
            Ctx.SaveChanges();

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
            temp.AddRange(Ctx.TempContent.Select(s => new AllTodayResult
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

        return [.. temp.OrderBy(o => o.TipusNev)];
    }

    public ContentResult? LoadPufferTodayContentByNid(string nid)
    {
        _logger.LogDebug("{Method} was called, {Nid}", nameof(LoadPufferTodayContentByNid), nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.TempContent.FirstOrDefault(g => g.Nid == guid);

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
                    Inserted = res.Inserted.ToString("yyyy-MM-dd HH:mm:ss"),
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
            var res = Ctx.Content.Where(g => g.Img != "" && g.Img != null)
                .Select(s => new ImgData { Img = s.Img, Cim = s.Cim, Tipus = ((Forras)s.Tipus).ToString() }).ToList();

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return [];
    }

    public IEnumerable<FileData> GetAudioList()
    {
        _logger.LogDebug("GetAudioList was called");

        try
        {
            var res = Ctx.Content.Where(g => g.FileUrl != "" && g.FileUrl != null)
                .Select(s => new FileData { Path = s.FileUrl, Cim = s.Cim, Tipus = ((Forras)s.Tipus).ToString() }).ToList();

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return [];
    }

    public LastTodayResult GetLastTodayContentByTipus(int tipus)
    {
        _logger.LogDebug("{Method} was called, {Tipus}", nameof(GetLastTodayContentByTipus), tipus);

        try
        {
            var date = Clock.Now.ToString("yyyy-MM-dd");
            var hoNap = Clock.Now.ToString("MM-dd");
            if (tipus == 7)
            {
                date = Clock.Now.ToString("yyyy-MM-01");
                hoNap = Clock.Now.ToString("MM-01");
            }

            ContentDetailsModel? res = null;

            if (!AndrokatConfiguration.FixContentTypeIds().Contains(tipus))
            {
                var firstByDateAndTipus = Ctx.Content.FirstOrDefault(g => g.Tipus == tipus && g.Fulldatum.StartsWith(date));
                if (firstByDateAndTipus is not null)
                    res = Mapper.Map<ContentDetailsModel>(firstByDateAndTipus);
            }
            else
            {
                var fix = Ctx.FixContent.FirstOrDefault(g => g.Tipus == tipus && g.Datum == hoNap);
                if (fix is not null)
                    res = Mapper.Map<ContentDetailsModel>(fix);
            }

            if (res is null)
            {
                var firstByTipus = Ctx.Content.Where(g => g.Tipus == tipus).OrderByDescending(o => o.Fulldatum).FirstOrDefault();
                if (firstByTipus is not null)
                    res = Mapper.Map<ContentDetailsModel>(firstByTipus);
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

    public IEnumerable<AllRecordResult> GetAllContentByTipus(int tipus)
    {
        _logger.LogDebug("{Name} was called, {Tipus}", nameof(GetAllContentByTipus), tipus);

        var res = Ctx.Content
            .Where(g => g.Tipus == tipus).Select(s => new AllRecordResult { Nid = s.Nid, Datum = s.Fulldatum })
            .OrderBy(o => o.Datum).ToList();

        return res;
    }

    public Dictionary<int, string> GetAllContentTipusFromDb()
    {
        _logger.LogDebug("{Name} was called", nameof(GetAllContentTipusFromDb));

        var tipusok = new Dictionary<int, string>();
        Ctx.Content.GroupBy(p => p.Tipus).Select(g =>
            new { tipus = g.Key, count = g.Count() }).ToList().ForEach(w =>
            {
                var tipus = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(w.tipus);
                tipusok.Add(w.tipus, tipus.TipusNev);
            });

        return tipusok;
    }

    public IEnumerable<AllRecordResult> GetAllMaiSzentByMonthResult(string date)
    {
        _logger.LogDebug("GetAllMaiSzentByMonthResult was called, {Date}", date);

        var res = Ctx.MaiSzent
            .Where(g => g.Datum.StartsWith(date)).Select(s => new AllRecordResult { Nid = s.Nid, Datum = s.Datum })
            .OrderBy(o => o.Datum).ToList();

        return res;
    }

    public IEnumerable<AllRecordResult> GetAllImaByCsoportResult(string csoport)
    {
        _logger.LogDebug("GetAllImaByCsoportResult was called, {Csoport}", csoport);

        var res = Ctx.ImaContent
            .Where(g => g.Csoport == csoport).Select(s => new AllRecordResult { Nid = s.Nid, Csoport = s.Cim })
            .OrderBy(o => o.Csoport).ToList();

        return res;
    }

    public IEnumerable<AllRecordResult> GetAllRadioResult()
    {
        _logger.LogDebug("GetAllRadioResult was called");

        var res = Ctx.RadioMusor
            .Select(s => new AllRecordResult { Nid = s.Nid, Csoport = s.Source })
            .OrderBy(o => o.Csoport).ToList();

        return res;
    }

    public RadioResult? LoadRadioByNid(string nid)
    {
        _logger.LogDebug("LoadRadioByNid was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.RadioMusor.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                return new RadioResult
                {
                    Musor = res.Musor,
                    Source = res.Source,
                    Nid = res.Nid.ToString(),
                    Inserted = res.Inserted
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return null;
    }

    public ContentResult? LoadImaByNid(string nid)
    {
        _logger.LogDebug("LoadImaByNid was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.ImaContent.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                return new ContentResult
                {
                    Cim = res.Cim,
                    FullDatum = res.Datum.ToString("yyyy-MM-dd HH:mm:ss"),
                    Idezet = res.Szoveg,
                    Img = "",
                    Nid = res.Nid.ToString(),
                    Def = "",
                    Inserted = res.Datum.ToString("yyyy-MM-dd HH:mm:ss")
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return null;
    }

    public ContentResult? LoadMaiSzentByNid(string nid)
    {
        _logger.LogDebug("LoadMaiSzentByNid was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.MaiSzent.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                return new ContentResult
                {
                    Cim = res.Cim,
                    FullDatum = res.Datum,
                    Idezet = res.Idezet,
                    Img = res.Img,
                    Nid = res.Nid.ToString(),
                    Def = _androkatConfiguration.Value.GetContentMetaDataModelByTipus((int)Forras.maiszent).TipusNev,
                    Inserted = res.Inserted.ToString("yyyy-MM-dd HH:mm:ss")
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return null;
    }

    public ContentResult? LoadTodayContentByNid(string nid)
    {
        _logger.LogDebug("LoadTodayContentByNid was called, {Nid}", nid);

        try
        {
            var guid = Guid.Parse(nid);
            var res = Ctx.Content.FirstOrDefault(f => f.Nid == guid);
            if (res is not null)
            {
                return new ContentResult
                {
                    Cim = res.Cim,
                    Forras = res.Forras,
                    Idezet = res.Idezet,
                    FileUrl = res.FileUrl,
                    Img = res.Img,
                    Nid = res.Nid.ToString(),
                    Def = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(res.Tipus).TipusNev,
                    Tipus = res.Tipus,
                    FullDatum = res.Fulldatum,
                    Inserted = res.Inserted.ToString("yyyy-MM-dd HH:mm:ss")
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return null;
    }

    public AdminAResult GetAdminAResult(bool isAdvent, bool isNagyBojt)
    {
        _logger.LogDebug("GetAdminAResult was called");

        var result = new AdminAResult();

        try
        {
            var date = Clock.Now.ToString("yyyy-MM-dd");
            var hoNap = Clock.Now.ToString("MM-dd");

            var focolareDate = Clock.Now.ToString("yyyy-MM-01");

            var res = new List<ContentDetailsModel>();
            Ctx.Content.Where(w => w.Fulldatum.StartsWith(date) || w.Tipus == 7 && w.Fulldatum.StartsWith(focolareDate)).OrderBy(o => o.Tipus).ToList()
                .ForEach(f => res.Add(Mapper.Map<ContentDetailsModel>(f)));

            var fix = Ctx.FixContent.Where(w => w.Datum == hoNap).OrderBy(o => o.Tipus).ToList();
            fix.ForEach(f => { res.Add(Mapper.Map<ContentDetailsModel>(f)); });

            var maiszent = Ctx.MaiSzent.Where(w => w.Datum == hoNap).ToList();
            maiszent.ForEach(f =>
            {
                res.Add(Mapper.Map<ContentDetailsModel>(f));
            });

            var maiAnyagok = new List<int>();
            var maiAnyagokHtml = GetMaiAnyagok(res, maiAnyagok, "<strong>Mai anyagok</strong><br>");

            var osszesTipus = new List<int>();
            osszesTipus.AddRange(AndrokatConfiguration.ContentTypeIds());
            osszesTipus.AddRange(AndrokatConfiguration.FixContentTypeIds());

            if (!isAdvent)
                osszesTipus.Remove((int)Forras.advent);
            if (!isNagyBojt)
                osszesTipus.Remove((int)Forras.nagybojt);

            var maiHianyzok = osszesTipus.Except(maiAnyagok.OrderBy(o => o)).ToList();

            var hianyzokHtml = GetMaiHianyzok(date, focolareDate, maiHianyzok);

            result.MaiAnyagok = hianyzokHtml + maiAnyagokHtml;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return result;
    }

    private List<ContentDetailsModel> GetActualMaiSzent()
    {
        var list = new List<ContentDetailsModel>();
        var hoNap = Clock.Now.ToString("MM-dd");
        var month = Clock.Now.ToString("MM");

        var rows = Ctx.MaiSzent.AsNoTracking().Where(w => w.Datum == hoNap);
        if (!rows.Any())
        {
            var rows2 = Ctx.MaiSzent.AsNoTracking().AsEnumerable()
            .Where(w => w.Datum.StartsWith($"{month}-") && w.Fulldatum < Clock.Now)
            .OrderByDescending(o => o.Datum).Take(1).ToList();

            if (rows2.Count == 0)
            {
                var prevmonth = Clock.Now.AddMonths(-1).ToString("MM");
                //nincs az új hónap első napján anyag
                rows2 = Ctx.MaiSzent.AsNoTracking().AsEnumerable()
                    .Where(w => w.Datum.StartsWith($"{prevmonth}-") && w.Fulldatum < Clock.Now)
                    .OrderByDescending(o => o.Datum).Take(1).ToList();
            }

            rows2.ForEach(row =>
            {
                list.Add(Mapper.Map<ContentDetailsModel>(row));
            });
        }
        else
            rows.ToList().ForEach(row =>
            {
                list.Add(Mapper.Map<ContentDetailsModel>(row));
            });

        return list;
    }

    private string GetMaiHianyzok(string date, string focolareDate, List<int> list)
    {
        var sb = new StringBuilder();
        sb.Append("<strong>Ma hiányzó anyagok</strong><br>");

        if (list.Contains((int)Forras.maiszent))
        {
            var szent = GetActualMaiSzent().FirstOrDefault();
            if (szent is not null)
            {
                var data = _androkatConfiguration.Value.GetContentMetaDataModelByTipus((int)Forras.maiszent);
                sb.Append(" " + (int)Forras.maiszent + " - <a href='" + data.Link + "' target='_blank'>" + data.TipusNev + "</a><br>");
                sb.Append("[" + szent.Fulldatum + "] - " + szent.Cim + "<br><br>");
            }
        }

        foreach (var item in list.Where(w => w != (int)Forras.maiszent).OrderBy(o => o))
        {
            var data = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item);

            sb.Append(" " + item + " - <a href='" + data.Link + "' target='_blank'>" + data.TipusNev + "</a><br>");

            var result = GetHianyzokAdatai(date, focolareDate, item);
            if (!string.IsNullOrWhiteSpace(result))
                sb.Append(result + "<br><br>");
        }

        return sb.ToString();
    }

    private string GetHianyzokAdatai(string date, string focolareDate, int item)
    {
        var datum = item == 7 ? focolareDate : date;
        var res2 = Ctx.Content.FirstOrDefault(w => w.Tipus == item && w.Fulldatum.StartsWith(datum));

        //ezek nem mindig naponta jelennek meg, igy visszaadjuk a legutobbit, ha van                
        res2 ??= Ctx.Content.Where(w => w.Tipus == item).OrderByDescending(o => o.Inserted).FirstOrDefault();
        if (res2 is null)
            return "";

        var res = "[" + res2.Fulldatum + "] - " + res2.Cim;
        if (item != 6 && item != 15 && item != 28 && item != 38 && item != 39 && item != 60)
            return res;

        if (string.IsNullOrWhiteSpace(res2.FileUrl))
            res += "<br><a href='" + res2.Idezet + "'>" + res2.Idezet + "</a>";
        else
            res += "<br><a href='" + res2.FileUrl + "'>" + res2.FileUrl + "</a>";

        return res;
    }

    private string GetMaiAnyagok(IEnumerable<ContentDetailsModel> res, List<int> list1, string resString1)
    {
        var sb = new StringBuilder();
        sb.Append(resString1);
        foreach (var item in res.OrderBy(o => o.Tipus))
        {
            if (list1.Contains(item.Tipus))
                continue;

            var data = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus);
            sb.Append(" " + item.Tipus + " - <a href='" + data.Link + "' target='_blank'>" + data.TipusNev + "</a><br>");

            sb.Append("[" + item.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss") + "] - " + item.Cim + "<br>");
            if (item.Tipus is 6 or 15 or 28 or 38 or 39 or 60)
            {
                if (string.IsNullOrWhiteSpace(item.FileUrl))
                    sb.Append("<a href='" + item.Idezet + "'>" + item.Idezet + "</a><br>");
                else
                    sb.Append("<a href='" + item.FileUrl + "'>" + item.FileUrl + "</a><br>");
            }

            sb.Append("<br>");

            list1.Add(item.Tipus);
        }

        return sb.ToString();
    }

    public AdminBResult GetAdminBResult()
    {
        _logger.LogDebug("GetAdminBResult was called");

        var adminResult = new AdminBResult();

        try
        {
            var today = Clock.Now.DateTime.ToString("MM-dd");
            var res = Ctx.RadioMusor.Count(w => w.Inserted.Contains(today));
            adminResult.Radio = res == 3 ? "radio: OK" : "radio: <span style='color:red;'>NOT OK</span> #: " + res;

            Ctx.RadioMusor.ToList().ForEach(w =>
            {
                adminResult.RadioList += w.Source + " (" + w.Inserted + ")<br>";
            });

            //törölni to do
            var isAdvent = Ctx.SystemInfo.FirstOrDefault(w => w.Key == Constants.IsAdvent);
            if (isAdvent is null)
            {
                Ctx.SystemInfo.Add(new SystemInfo { Key = Constants.IsAdvent, Value = "true" });
                Ctx.SystemInfo.Add(new SystemInfo { Key = Constants.IsNagyBojt, Value = "false" });
                Ctx.SaveChanges();
            }

            var vatikan = Ctx.SystemInfo.Where(w => w.Key == "radio").Select(s => s.Value).First();
            var radios = JsonSerializer.Deserialize<Dictionary<string, string>>(vatikan!)!;

            var sb = new StringBuilder();
            foreach (var item in radios.Where(item => item.Key == "vatikan"))
            {
                sb.Append(item.Key + " (" + item.Value + ")");
            }
            adminResult.RadioList += sb.ToString();

            var countOfTipusok = new Dictionary<int, string>();
            Ctx.Content.GroupBy(p => p.Tipus).Select(g =>
            new { tipus = g.Key, count = g.Count() }).ToList().ForEach(w =>
            {
                var tipus = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(w.tipus);
                countOfTipusok.Add(w.tipus, " " + tipus.TipusNev + " #: " + w.count + "<br>");
            });
            Ctx.FixContent.GroupBy(p => p.Tipus).Select(g =>
            new { tipus = g.Key, count = g.Count() }).ToList().ForEach(w =>
            {
                var tipus = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(w.tipus);
                countOfTipusok.Add(w.tipus, " " + tipus.TipusNev + " #: " + w.count + "<br>");
            });
            var szent = Ctx.MaiSzent.Count();
            var szenttipus = _androkatConfiguration.Value.GetContentMetaDataModelByTipus((int)Forras.maiszent);
            countOfTipusok.Add((int)Forras.maiszent, " " + szenttipus.TipusNev + " #: " + szent + "<br>");

            countOfTipusok = countOfTipusok.OrderBy(o => o.Key).ToDictionary(t => t.Key, t => t.Value);
            var sbCount = new StringBuilder();
            foreach (var item in countOfTipusok)
            {
                sbCount.Append(item.Key + item.Value);
            }
            adminResult.CountOfTipusok = sbCount.ToString();

            var sysRes = Ctx.SystemInfo.ToList();

            if (sysRes.Count == 0)
                return adminResult;

            adminResult.SysTable = GetSysTable(sysRes);

            return adminResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return adminResult;
    }

    public IEnumerable<SystemInfoData> GetAllSystemInfo()
    {
        _logger.LogDebug("GetAllRadioResult was called");

        var res = Ctx.SystemInfo.Select(s => new SystemInfoData
        {
            Id = s.Id,
            Key = s.Key,
            Value = s.Value
        }).ToList();

        return res;
    }

    private static string GetSysTable(List<SystemInfo> sysRes)
    {
        var sys = new StringBuilder();
        foreach (var value in sysRes)
        {
            if (value.Key is "website" or "radio")
            {
                var web = JsonSerializer.Deserialize<Dictionary<string, string>>(value.Value)!;
                sys.Append("[" + value.Id + "] - " + value.Key);

                foreach (var item in web.Select(item => item.Value))
                {
                    sys.Append("<br>&nbsp;&nbsp;&nbsp;<a href=" + item + " target=\"_blank\">" + item + "</a>");
                }
                sys.Append("'<br>");
            }
            else
                sys.Append("[" + value.Id + "] - " + value.Key + " - " + value.Value + "<br>");
        }

        return sys.ToString();
    }

    public AdminResult GetAdminResult()
    {
        _logger.LogDebug("GetAdminResult was called");

        var adminResult = new AdminResult();

        try
        {
            var today = Clock.Now.DateTime.ToString("yyyy-MM-dd");

            var list = new List<int>
            {
                (int)Forras.keresztenyelet,
                (int)Forras.kurir,
                (int)Forras.bonumtv
            };

            var res = Ctx.Content.Count(w => list.Contains(w.Tipus) && w.Fulldatum.Contains(today));
            adminResult.Header += res == 0 ? "news: <span style='color:red;'>NOT OK</span> #: " + res + " | " : "";

            list =
            [
                (int)Forras.bzarandokma,
                (int)Forras.b777,
                (int)Forras.jezsuitablog
            ];

            res = Ctx.Content.Count(w => list.Contains(w.Tipus) && w.Fulldatum.Contains(today));
            adminResult.Header += res == 0 ? " | blog: <span style='color:red;'>NOT OK</span> #: " + res + " | " : "";

            res = Ctx.VideoContent.Count();
            adminResult.Header += "video: #: " + res + " | ";

            res = Ctx.ImaContent.Count();
            adminResult.Header += "ima: #: " + res + " | ";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return adminResult;
    }

    public List<SystemInfoData> GetIsAdventAndNagybojt()
    {
        return [.. Ctx.SystemInfo.Where(w => w.Key == Constants.IsAdvent || w.Key == Constants.IsNagyBojt).AsNoTracking().Select(s => new SystemInfoData
        {
            Key = s.Key,
            Value = s.Value,
            Id = s.Id
        })];
    }
}