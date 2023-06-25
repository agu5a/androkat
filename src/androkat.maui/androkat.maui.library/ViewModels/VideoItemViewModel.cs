using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class VideoItemViewModel : BaseView
{
    public VideoEntity VideoEntity { get; set; }
    private readonly IBrowser _browser;

    public VideoItemViewModel(VideoEntity contentEntity, IBrowser browser)
    {
        VideoEntity = contentEntity;
        _browser = browser;
    }

    [RelayCommand]
    async Task OpenVideoInBrowser()
    {
        var browserOptions = new BrowserLaunchOptions
        {
            //PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
            //PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
        };

        await _browser.OpenAsync(VideoEntity.Link, browserOptions);
    }
}