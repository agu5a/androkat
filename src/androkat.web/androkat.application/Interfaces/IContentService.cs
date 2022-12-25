using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IContentService
{
    IReadOnlyCollection<ContentModel> GetAjanlat();
    IReadOnlyCollection<AudioViewModel> GetAudio();
    IReadOnlyCollection<ContentModel> GetHome();    
    IReadOnlyCollection<ContentModel> GetImaPage(string csoport);
    IReadOnlyCollection<ContentModel> GetSzentek();
    IReadOnlyCollection<VideoSourceViewModel> GetVideoSourcePage();
}