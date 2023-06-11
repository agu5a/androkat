using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class VideoItemViewModel : BaseView
{
    public VideoDto VideoDto { get; set; }
    private readonly IBrowser _browser;

    public VideoItemViewModel(VideoDto contentDto, IBrowser browser)
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