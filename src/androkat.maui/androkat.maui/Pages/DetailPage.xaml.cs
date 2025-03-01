using androkat.maui.library.Models;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailPage
{
    private DetailViewModel ViewModel => (BindingContext as DetailViewModel)!;

    public DetailPage(DetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        MyWebView.Navigating += OnWebViewNavigating;
    }

    #pragma warning disable S2325
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void OnWebViewNavigating(object? sender, WebNavigatingEventArgs e)
    #pragma warning restore S2325
    {
        e.Cancel = true;

        if (!e.Url.StartsWith("appscheme://"))
        {
            return;
        }
        
        var actualUrl = e.Url.Replace("appscheme://", string.Empty);
        Browser.Default.OpenAsync(actualUrl, BrowserLaunchMode.SystemPreferred);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
        await LoadContentImageAsync();
    }

    private async Task LoadContentImageAsync()
    {
        try
        {
            var imageUrl = ViewModel.ContentView.contentImg;
            
            if (!string.IsNullOrEmpty(imageUrl))
            {
                using var httpClient = new HttpClient();
                var folder = "images/szentek";

                if (ViewModel.ContentView.type is Activities.szeretetujsag or Activities.ajanlatweb)
                {
                    folder = "images/ajanlatok";
                }
                
                imageUrl = "https://androkat.hu/" + folder + "/" + imageUrl;
                var imageData = await httpClient.GetByteArrayAsync(imageUrl);
                
                ContentImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
                ContentImage.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            // Log error or handle exception
            System.Diagnostics.Debug.WriteLine($"Error loading image: {ex.Message}");
            ContentImage.IsVisible = false;
        }
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }
}