using androkat.hu.Models;
using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class WebPage : ContentPage
{
    private readonly IBrowser _browser;

    public WebPage(WebViewModel viewModel, IBrowser browser)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _browser = browser;

        webPicker.ItemsSource = viewModel.WebPageUrls;
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