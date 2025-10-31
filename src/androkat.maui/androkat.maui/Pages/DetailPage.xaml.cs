using androkat.hu.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailPage
{
    private DetailViewModel ViewModel => (BindingContext as DetailViewModel)!;
    private ToolbarItem? _deleteFavoriteToolbarItem;

    public DetailPage(DetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        MyWebView.Navigating += OnWebViewNavigating;
    }

#pragma warning disable S2325
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private async void OnWebViewNavigating(object? sender, WebNavigatingEventArgs e)
#pragma warning restore S2325
    {
        if (!e.Url.StartsWith("appscheme://", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        e.Cancel = true;

        var actualUrl = e.Url.Replace("appscheme://", string.Empty, StringComparison.OrdinalIgnoreCase);

        // Fix malformed URLs (e.g., "https//" instead of "https://")
        if (actualUrl.StartsWith("http//", StringComparison.OrdinalIgnoreCase))
        {
            actualUrl = actualUrl.Replace("http//", "http://", StringComparison.OrdinalIgnoreCase);
        }
        if (actualUrl.StartsWith("https//", StringComparison.OrdinalIgnoreCase))
        {
            actualUrl = actualUrl.Replace("https//", "https://", StringComparison.OrdinalIgnoreCase);
        }

        try
        {
            await Shell.Current.GoToAsync($"WebViewPage?Url={Uri.EscapeDataString(actualUrl)}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error navigating to WebViewPage: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
        await LoadContentImageAsync();

        // Apply font scaling to WebView content
        if (ViewModel.ContentView?.ContentEntity?.Idezet != null)
        {
            var htmlContent = HtmlHelper.WrapHtmlWithFontScale(ViewModel.ContentView.ContentEntity.Idezet);
            MyWebView.Source = new HtmlWebViewSource { Html = htmlContent };
        }

        // Control toolbar item visibility
        UpdateToolbarItems();
    }

    private void UpdateToolbarItems()
    {
        if (ViewModel.ShowDeleteFavoriteButton)
        {
            // Create and add the delete toolbar item if it doesn't exist
            if (_deleteFavoriteToolbarItem == null)
            {
                _deleteFavoriteToolbarItem = new ToolbarItem
                {
                    IconImageSource = "delete",
                    Text = "Törlés kedvencekből"
                };
                _deleteFavoriteToolbarItem.SetBinding(ToolbarItem.CommandProperty, "DeleteFavoriteCommand");
            }

            if (!ToolbarItems.Contains(_deleteFavoriteToolbarItem))
            {
                ToolbarItems.Insert(2, _deleteFavoriteToolbarItem); // Insert after favorite, before send
            }
        }
        else
        {
            // Remove the delete toolbar item if it exists
            if (_deleteFavoriteToolbarItem != null)
            {
                ToolbarItems.Remove(_deleteFavoriteToolbarItem);
            }
        }
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

#pragma warning disable S1075 // URIs should not be concatenated
                imageUrl = "https://androkat.hu/" + folder + '/' + imageUrl;
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