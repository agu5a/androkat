using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace androkat.application.Service;

public class ApiService : IApiService
{
    private readonly IClock _iClock;
    private readonly IMapper _mapper;

    public ApiService(IClock iClock, IMapper mapper)
    {
        _iClock = iClock;
        _mapper = mapper;
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
}