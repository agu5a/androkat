using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class VideoItemViewModel(VideoEntity contentEntity, IBrowser browser) : BaseView
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public VideoEntity VideoEntity { get; set; } = contentEntity;

    public string FormattedDate => VideoEntity.Datum.ToString("yyyy.MM.dd");

    [RelayCommand]
    async Task OpenVideoInBrowser()
    {
        var browserOptions = new BrowserLaunchOptions
        {
            //PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
            //PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
        };

        await browser.OpenAsync(VideoEntity.Link, browserOptions);
    }
}