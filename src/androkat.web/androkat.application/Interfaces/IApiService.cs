using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IApiService
{    
    IReadOnlyCollection<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache);
    IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset, VideoCache videoCache);
    string GetVideoForWebPage(string f, int offset, VideoCache videoCache);
}