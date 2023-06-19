using androkat.maui.library.Abstraction;
using androkat.maui.library.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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
        if (ViewModel.FavoriteCount > 0)
            return $"Kedvencek ({ViewModel.FavoriteCount})";
        else
            return $"Kedvencek";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var isDelete = await Shell.Current.DisplayAlert("Törlés", "Biztos törlöd?", "Igen", "Nem");
        if (isDelete)
        {
            await _pageService.DeleteAllFavorite();
            ViewModel.PageTitle = "Kedvencek";
            await ViewModel.InitializeAsync();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Kedvencek adatbázis sikeresen törölve", ToastDuration.Short, 14d);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}