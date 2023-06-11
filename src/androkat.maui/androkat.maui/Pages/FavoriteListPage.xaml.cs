using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class FavoriteListPage : ContentPage
{
    private FavoriteListViewModel ViewModel => BindingContext as FavoriteListViewModel;

    public FavoriteListPage(FavoriteListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
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
        return $"Kedvencek {ViewModel.FavoriteCount}";
    }
}