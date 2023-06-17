using androkat.maui.library.Abstraction;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class FavoriteListPage : ContentPage
{
    private readonly IPageService _pageService;
    private FavoriteListViewModel ViewModel => BindingContext as FavoriteListViewModel;

    public FavoriteListPage(FavoriteListViewModel viewModel, IPageService pageService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _pageService = pageService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await ViewModel.InitializeAsync();
        ViewModel.PageTitle = GetPageTitle();
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private string GetPageTitle()
    {
        return $"Kedvencek ({ViewModel.FavoriteCount})";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await _pageService.DeleteAllFavorite();
    }
}