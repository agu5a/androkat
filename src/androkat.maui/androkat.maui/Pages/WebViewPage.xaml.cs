namespace androkat.hu.Pages;

[QueryProperty(nameof(Url), nameof(Url))]
public partial class WebViewPage : ContentPage
{
    private string _url = string.Empty;

    public string Url
    {
        get => _url;
        set
        {
            _url = value;
            if (!string.IsNullOrEmpty(_url) && WebViewBrowser != null)
            {
                WebViewBrowser.Source = _url;
            }
        }
    }

    public WebViewPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!string.IsNullOrEmpty(_url))
        {
            WebViewBrowser.Source = _url;
        }
    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        LoadingIndicator.IsRunning = LoadingIndicator.IsVisible = false;

        if (e.Result == WebNavigationResult.Success)
        {
            System.Diagnostics.Debug.WriteLine($"WebView navigated to {e.Url} successfully.");
        }
        else
        {
            await DisplayAlert("Navigáció sikertelen", e.Result.ToString(), "OK");
        }
    }
}
