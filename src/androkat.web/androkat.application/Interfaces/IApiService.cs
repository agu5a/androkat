using androkat.domain.Model.ContentCache;
using androkat.domain.Model.WebResponse;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IApiService
{
    IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset, VideoCache videoCache);
    IEnumerable<RadioMusorResponse> GetRadioBySource(string s, BookRadioSysCache bookRadioSysCache);
}