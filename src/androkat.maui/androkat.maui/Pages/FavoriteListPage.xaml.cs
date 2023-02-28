using AndroidX.Lifecycle;
using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class FavoriteListPage : ContentPage
{
    private FavoriteListViewModel _viewModel => BindingContext as FavoriteListViewModel;

    public FavoriteListPage(ContentListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;    
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();        
        //await _viewModel.InitializeAsync();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _viewModel.PageTitle = GetPageTitle();
        await _viewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        //_viewModel.Contents.Clear();
        //Navigation.RemovePage(this);
    }

    private string GetPageTitle()
    {
        return "Kedvencek {_viewModel}";
    }    
}