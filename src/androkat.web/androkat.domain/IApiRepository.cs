using androkat.domain.Model;
using System;
using System.Collections.Generic;

namespace androkat.domain;

public interface IApiRepository
{
    bool AddContentDetailsModel(ContentDetailsModel contentDetailsModel);
    bool AddTempContent(ContentDetailsModel contentDetailsModel);
    bool AddVideo(VideoModel videoModel);
    bool DeleteContentDetailsByNid(Guid nid);
    bool DeleteVideoByNid(Guid nid);
    IEnumerable<ContentDetailsModel> GetContentDetailsModels();
    IEnumerable<SystemInfoModel> GetSystemInfoModels();
    IEnumerable<VideoModel> GetVideoModels();
    bool UpdateRadioMusor(RadioMusorModel radioMusorModel);
    bool UpdateRadioSystemInfo(string value);
}