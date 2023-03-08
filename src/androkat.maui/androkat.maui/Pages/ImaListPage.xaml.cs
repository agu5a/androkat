using AndroidX.Lifecycle;
using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class ImaListPage : ContentPage
{
    private ImaListViewModel _viewModel => BindingContext as ImaListViewModel;

    public ImaListPage(ImaListViewModel vm)
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
        _viewModel.PageTitle = "Imádságok";// GetPageTitle();
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    //private string GetPageTitle()
    //{
    //    return $"Kedvencek {_viewModel.FavoriteCount}";
    //}    
}