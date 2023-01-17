using androkat.domain.Model.WebResponse;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IApiService
{
    IReadOnlyCollection<VideoResponse> GetVideoByOffset(int offset);
}