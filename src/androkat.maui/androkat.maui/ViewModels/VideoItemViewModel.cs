using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class VideoItemViewModel : BaseView
{
    public VideoEntity VideoDto { get; set; }
    private readonly IBrowser _browser;

    public VideoItemViewModel(VideoEntity contentDto, IBrowser browser)
    {
        VideoDto = contentDto;
        _browser = browser;
    }

    [RelayCommand]
    async Task NavigateToDetail()
    {
        var browserOptions = new BrowserLaunchOptions
        {
            //PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
            //PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
        };

        await _browser.OpenAsync(VideoDto.Link, browserOptions);
    }
}