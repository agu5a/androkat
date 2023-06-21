using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ContentListPage : ContentPage
{
    private ContentListViewModel ViewModel => BindingContext as ContentListViewModel;
    private int _stackCount = 0;

    public ContentListPage(ContentListViewModel viewModel)
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
        // Hack: Get the category Id
        ViewModel.Id = GetCategoryIdFromRoute();
        ViewModel.PageTitle = GetPageTitle(ViewModel.Id);
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        await ViewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    protected override bool OnBackButtonPressed()
    {
        return base.OnBackButtonPressed();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _stackCount = Application.Current.MainPage.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
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
        return Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault();
    }
}