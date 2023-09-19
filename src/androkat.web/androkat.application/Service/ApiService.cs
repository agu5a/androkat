using androkat.application.Interfaces;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace androkat.application.Service;

public class ApiService : IApiService
{
    private readonly IClock _iClock;

    public ApiService(IClock iClock)
    {
        _iClock = iClock;
    }

    public IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset, VideoCache videoCache)
    {
        var temp = new List<VideoResponse>();
        foreach (var item in videoCache.Video.OrderByDescending(o => o.Date).Skip(offset).Take(5))
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

        return temp.AsReadOnly();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "<Pending>")]
    public ImaResponse GetImaByDate(string date, ImaCache imaCache)
    {
        _ = DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("hu-HU"), out DateTime datum);

        var temp = new ImaResponse
        {
            HasMore = false,
            Imak = new List<ImaDetailsResponse>()
        };

        var index = 0;
        foreach (var item in imaCache.Imak.Where(w => w.Datum > datum))
        {
            if (index == 10)
            {
                temp.HasMore = true;
                break;
            }

            if (int.TryParse(item.Csoport, out int cs))
            {
                temp.Imak.Add(new ImaDetailsResponse
                {
                    Cim = item.Cim,
                    Csoport = cs,
                    Leiras = item.Szoveg,
                    Nid = item.Nid,
                    Time = item.Datum.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            index++;
        }
        return temp;
    }

    public IReadOnlyCollection<SystemDataResponse> GetSystemData(BookRadioSysCache bookRadioSysCache)
    {
        return bookRadioSysCache.SystemData.Select(s => new SystemDataResponse { Key = s.Key, Value = s.Value }).ToList();
    }

    public IReadOnlyCollection<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache)
    {
        return bookRadioSysCache.RadioMusor
            .Where(w => w.Source == s)
            .Select(s => new RadioMusorResponse { Musor = s.Musor }).ToList();
    }

    public IReadOnlyCollection<ContentResponse> GetContentByTipusAndNid(int tipus, Guid n, MainCache mainCache)
    {
        var temp = new List<ContentResponse>();

        if (tipus == 58 || tipus == 25)
        {
            GetHumorAndAjandek(n, tipus, mainCache, temp);
        }
        else
        {
            foreach (var item in mainCache.ContentDetailsModels.Where(w => w.Tipus == tipus))
            {
                //a user már letöltötte, nem kell újból
                if (n != Guid.Empty && n == item.Nid)
                    break;

                temp.Add(new ContentResponse
                {
                    Cim = item.Cim,
                    Datum = item.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss"),
                    Forras = item.Forras ?? string.Empty,
                    Idezet = string.IsNullOrEmpty(item.FileUrl) ? item.Idezet : item.FileUrl,
                    Img = item.Img ?? string.Empty,
                    Nid = item.Nid,
                    KulsoLink = string.Empty
                });
            }
        }
        return temp;
    }

    public string GetVideoForWebPage(string f, int offset, VideoCache videoCache)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(f))
        {
            foreach (var item in videoCache.Video.Where(w => w.ChannelId == f).OrderByDescending(o => o.Date).Skip(offset).Take(4))
            {
                GetVideoHtml(sb, item);
            }
            return sb.ToString();
        }

        foreach (var item in videoCache.Video.OrderByDescending(o => o.Date).Skip(offset).Take(4))
        {
            GetVideoHtml(sb, item);
        }

        return sb.ToString();
    }

    private static void GetVideoHtml(StringBuilder sb, VideoModel item)
    {
        sb.Append("<div class='col mb-1'>");
        sb.Append("<div class=\"videoBox p-3 border bg-light\" style=\"border-radius: 0.25rem;\"><h5><a href=\"" + item.VideoLink + "\" target =\"_blank\">" + item.Cim + "</a></h5>");
        sb.Append("<div><strong>Forrás</strong>: " + item.Forras + "</div>");
        sb.Append("<div>" + item.Date + "</div>");
        sb.Append("<div class=\"video-container\" ><iframe src=\"" + item.VideoLink.Replace("watch?v=", "embed/") + "\" width =\"310\" height =\"233\" title=\"YouTube video player\" " +
            "frameborder =\"0\" allow=\"accelerometer;\"></iframe></div>");
        sb.Append("</div></div>");
    }

    private void GetHumorAndAjandek(Guid n, int tipus, MainCache m, List<ContentResponse> temp)
    {
        string date = _iClock.Now.ToString("yyyy-MM-dd");
        var todayContent = m.ContentDetailsModels.FirstOrDefault(w => w.Tipus == tipus && w.Fulldatum.ToString("yyyy-MM-dd").StartsWith(date));
        todayContent ??= m.ContentDetailsModels.Where(w => w.Tipus == tipus).OrderByDescending(o => o.Inserted).FirstOrDefault();

        if (todayContent is not null)
        {
            var downloaded = false;

            //a user már letöltötte, nem kell újból
            if (n != Guid.Empty && n == todayContent.Nid)
                downloaded = true;

            if (!downloaded)
            {
                temp.Add(new ContentResponse
                {
                    Cim = todayContent.Cim,
                    Datum = todayContent.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss"),
                    Forras = todayContent.Forras ?? string.Empty,
                    Idezet = todayContent.Idezet,
                    Img = todayContent.Img ?? string.Empty,
                    Nid = todayContent.Nid,
                    KulsoLink = string.Empty
                });
            }
        }
    }
}