using androkat.hu.Models;
using androkat.hu.ViewModels;
using androkat.maui.library.Services;

namespace androkat.hu.Pages;

public partial class WebPage : ContentPage
{
    private readonly PageService _pageService;
    private readonly IBrowser _browser;

    public WebPage(WebViewModel vm, PageService pageService, IBrowser browser)
    {
        InitializeComponent();
        BindingContext = vm;
        _pageService = pageService;
        _browser = browser;

        webPicker.ItemsSource = vm.WebPageUrls;
        webPicker.ItemDisplayBinding = new Binding("Name");
    }

    private async Task OpenWebPageAsync(Uri url)
    {
        var browserOptions = new BrowserLaunchOptions
        {
            //PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
            //PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
        };

        await _browser.OpenAsync(url, browserOptions);
    }

    private void webPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            OpenWebPageAsync(new Uri(((WebUrl)picker.ItemsSource[selectedIndex]).Url)).Wait();
        }
    }
}