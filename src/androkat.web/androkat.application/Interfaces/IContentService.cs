using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IContentService
{
    IReadOnlyCollection<ContentModel> GetAjanlat();
    IReadOnlyCollection<AudioModel> GetAudio();
    IReadOnlyCollection<ContentModel> GetHome();    
    IReadOnlyCollection<ContentModel> GetImaPage(string csoport);
    IReadOnlyCollection<RadioModel> GetRadioPage();
    IReadOnlyCollection<ContentModel> GetSzentek();
    IReadOnlyCollection<VideoSourceModel> GetVideoSourcePage();
}