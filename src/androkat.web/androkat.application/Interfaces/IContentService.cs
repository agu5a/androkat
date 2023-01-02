using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IContentService
{
    IReadOnlyCollection<ContentModel> GetAjanlat();
    IReadOnlyCollection<AudioModel> GetAudio();
    IReadOnlyCollection<ContentModel> GetBlog(int tipus);
    IReadOnlyCollection<ContentModel> GetHirek(int tipus);
    IReadOnlyCollection<ContentModel> GetHome();    
    IReadOnlyCollection<ContentModel> GetHumor();
    IReadOnlyCollection<ContentModel> GetImaPage(string csoport);
    IReadOnlyCollection<RadioModel> GetRadioPage();
    IReadOnlyCollection<ContentModel> GetSzent();
    IReadOnlyCollection<ContentModel> GetSzentek();
    IReadOnlyCollection<VideoSourceModel> GetVideoSourcePage();
}