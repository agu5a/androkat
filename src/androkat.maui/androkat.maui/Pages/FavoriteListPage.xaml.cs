using AndroidX.Lifecycle;
using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class FavoriteListPage : ContentPage
{
    private FavoriteListViewModel _viewModel => BindingContext as FavoriteListViewModel;

    public FavoriteListPage(FavoriteListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;    
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();        
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {        
        await _viewModel.InitializeAsync();
        _viewModel.PageTitle = GetPageTitle();
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private string GetPageTitle()
    {
        return $"Kedvencek {_viewModel.FavoriteCount}";
    }    
}