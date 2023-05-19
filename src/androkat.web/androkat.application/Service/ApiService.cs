using androkat.application.Interfaces;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using System.Collections.Generic;
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

    public IReadOnlyCollection<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache)
    {
        return bookRadioSysCache.RadioMusor
            .Where(w => w.Source == s)
            .Select(s => new RadioMusorResponse { Musor = s.Musor }).ToList();
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
}