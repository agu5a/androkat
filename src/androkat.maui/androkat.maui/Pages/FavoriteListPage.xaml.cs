using androkat.maui.library.Abstraction;
using androkat.maui.library.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace androkat.hu.Pages;

public partial class FavoriteListPage : ContentPage
{
    private readonly IPageService _pageService;
    private int _stackCount = 0;
    private FavoriteListViewModel ViewModel => BindingContext as FavoriteListViewModel;

    public FavoriteListPage(FavoriteListViewModel viewModel, IPageService pageService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _pageService = pageService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ViewModel.PageTitle = GetPageTitle();
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        await ViewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    private string GetPageTitle()
    {
        if (ViewModel.FavoriteCount > 0)
            return $"Kedvencek ({ViewModel.FavoriteCount})";
        else
            return $"Kedvencek";
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _stackCount = Application.Current.MainPage.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var isDelete = await Shell.Current.DisplayAlert("Törlés", "Biztos törlöd az összes kedvencet?", "Igen", "Nem");
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