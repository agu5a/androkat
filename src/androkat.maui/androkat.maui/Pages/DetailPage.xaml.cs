using androkat.maui.library.ViewModels;
using Microsoft.Maui.Controls;

namespace androkat.hu.Pages;

public partial class DetailPage : ContentPage
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
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }
}