using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using System;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IApiService
{
    ImaResponse GetImaByDate(string date, ImaCache imaCache);
    IReadOnlyCollection<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache);
    IReadOnlyCollection<ContentResponse> GetContentByTipusAndNid(int tipus, Guid n, MainCache mainCache);
    IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset, VideoCache videoCache);
    string GetVideoForWebPage(string f, int offset, VideoCache videoCache);
}