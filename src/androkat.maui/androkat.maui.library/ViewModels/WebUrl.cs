using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class WebUrl : ObservableObject
{
    private readonly IBrowser _browser;

    public WebUrl(string name, string url, IBrowser browser)
    {
        Name = name;
        Url = url;
        _browser = browser;
    }

    public string Name { get; }
    public string Url { get; }

    [RelayCommand]
    async Task NavigateToWeb()
    {
        var browserOptions = new BrowserLaunchOptions
        {
            //PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
            //PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
        };

        await _browser.OpenAsync(Url, browserOptions);
    }
}