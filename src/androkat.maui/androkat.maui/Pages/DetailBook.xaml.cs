using androkat.hu.Helpers;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailBook
{
    private BookDetailViewModel ViewModel => (BindingContext as BookDetailViewModel)!;
    private ToolbarItem? _deleteFavoriteToolbarItem;

    public DetailBook(BookDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeBookAsync();

        // Apply font scaling to WebView content
        if (ViewModel.ContentView?.ContentEntity?.Idezet != null && ViewModel.ShowWebView)
        {
            var htmlContent = HtmlHelper.WrapHtmlWithFontScale(ViewModel.ContentView.ContentEntity.Idezet);
            MyWebView.Source = new HtmlWebViewSource { Html = htmlContent };
        }

        // Control toolbar item visibility
        UpdateToolbarItems();
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
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
                ToolbarItems.Insert(0, _deleteFavoriteToolbarItem); // Insert before send
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
}