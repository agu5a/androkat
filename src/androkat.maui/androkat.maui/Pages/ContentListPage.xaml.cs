using androkat.hu.Views;
using androkat.maui.library.ViewModels;
using CommunityToolkit.Maui.Views;

namespace androkat.hu.Pages;

public partial class ContentListPage
{
    private ContentListViewModel ViewModel => (BindingContext as ContentListViewModel)!;
    private int _stackCount;
    private bool _showVisited = true;

    public ContentListPage(ContentListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Hack: Get the category Id
        var result = GetCategoryIdFromRoute();
        ViewModel.Id = string.IsNullOrEmpty(result) ? "34" : result;
        ViewModel.PageTitle = GetPageTitle(ViewModel.Id);
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _stackCount = Application.Current!.Windows[0].Page!.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        activityIndicator.IsRunning = true;
        activityIndicator.IsVisible = true;
        await ViewModel.InitializeAsync(_showVisited);
        activityIndicator.IsRunning = false;
        activityIndicator.IsVisible = false;
    }

    private async void OnFilterClicked(object sender, EventArgs e)
    {
        var filterView = new FilterView(_showVisited);
        filterView.FilterChanged += async (_, showVisited) =>
        {
            _showVisited = showVisited;
            await ViewModel.FetchAsync(_showVisited);
        };

        await this.ShowPopupAsync(filterView);
    }

    private static string GetPageTitle(string pageTypeId)
    {
        return pageTypeId switch
        {
            "1" => "KÖNYVAJÁNLÓ",
            "2" => "Mai Szent",
            "3" => "Szentek idézetei",
            "4" => "Katolikus Hírek",
            "5" => "Blog, magazin",
            "6" => "Nevessünk!",
            "7" => "Imádságok",
            "8" => "Hanganyagok",
            "11" => "Könyvolvasó",
            "34" => "Gyónáshoz elmélkedés",
            //Weboldalak
            _ => "Evangélium, elmélkedés", //0
        };
    }

    private static string GetCategoryIdFromRoute()
    {
        // Hack: As the shell can't define query parameters
        // in XAML, we have to parse the route. 
        // as a convention the last route section defines the category.
        // ugly but works for now :-(
        var result = Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault()!;
        return int.TryParse(result, out _) ? result : "";
    }
}