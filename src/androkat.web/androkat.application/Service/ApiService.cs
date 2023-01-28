using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using System;
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

public IEnumerable<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache)
    {
        foreach (var item in bookRadioSysCache.RadioMusor.Where(w => w.Source == s))
        {
            yield return new RadioMusorResponse
            {
                Musor = item.Musor
            };
        }
    }  
}