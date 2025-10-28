using androkat.hu.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailPray : ContentPage
{
    private PrayDetailViewModel ViewModel => (BindingContext as PrayDetailViewModel)!;
    private ToolbarItem? _deleteFavoriteToolbarItem;

    public DetailPray(PrayDetailViewModel viewModel)
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

        // Control toolbar item visibility for delete favorite button
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
                ToolbarItems.Insert(1, _deleteFavoriteToolbarItem); // Insert after text-to-speech, before share
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

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }
}
