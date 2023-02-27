using androkat.hu.Models;
using androkat.hu.Services;

namespace androkat.hu.ViewModels;

public partial class WebViewModel : ViewModelBase
{
    private readonly PageService _pageService;

    public WebViewModel(PageService pageService)
    {
        _pageService = pageService;

        WebPageUrls.Add(new WebUrl("archiv.katolikus.hu", "https://archiv.katolikus.hu/kek/"));
        WebPageUrls.Add(new WebUrl("szentiras.hu", "http://szentiras.hu/"));
        WebPageUrls.Add(new WebUrl("zsolozsma", "http://zsolozsma.katolikus.hu/"));
        WebPageUrls.Add(new WebUrl("miserend", "http://miserend.hu/"));
        WebPageUrls.Add(new WebUrl("szentter", "http://www.szentter.com/"));
        WebPageUrls.Add(new WebUrl("katolikus.tv", "https://katolikus.tv/elo-adas/"));
        WebPageUrls.Add(new WebUrl("liturgia", "https://liturgia.tv/"));        
    }

    public List<WebUrl> WebPageUrls = new();
}
