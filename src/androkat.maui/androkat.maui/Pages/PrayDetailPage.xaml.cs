using androkat.hu.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class PrayDetailPage : ContentPage
{
    private PrayDetailViewModel ViewModel => (BindingContext as PrayDetailViewModel)!;
    private ToolbarItem? _deleteFavoriteToolbarItem;

    public PrayDetailPage(PrayDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();

        // Set the WebView content with font scaling
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
        // Clear existing toolbar items
        ToolbarItems.Clear();

        // Add favorite/unfavorite button
        if (ViewModel.ShowDeleteFavoriteButton)
        {
            _deleteFavoriteToolbarItem = new ToolbarItem
            {
                Text = "Törlés",
                Command = ViewModel.DeleteFavoriteCommand,
                IconImageSource = "delete_favorite.png"
            };
            ToolbarItems.Add(_deleteFavoriteToolbarItem);
        }
        else
        {
            var addFavoriteItem = new ToolbarItem
            {
                Text = "Kedvenc",
                Command = ViewModel.AddFavoriteCommand,
                IconImageSource = "favorite.png"
            };
            ToolbarItems.Add(addFavoriteItem);
        }

        // Add text-to-speech button
        var speakItem = new ToolbarItem
        {
            Text = "Felolvasás",
            Command = ViewModel.StartTextToSpeechCommand,
            IconImageSource = "speak.png"
        };
        ToolbarItems.Add(speakItem);

        // Add share button
        var shareItem = new ToolbarItem
        {
            Text = "Megosztás",
            Command = ViewModel.ShareContentCommand,
            IconImageSource = "share.png"
        };
        ToolbarItems.Add(shareItem);
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }
}
