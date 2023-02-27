using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class ContentListPage : ContentPage
{
    private ContentListViewModel _viewModel => BindingContext as ContentListViewModel;

    public ContentListPage(ContentListViewModel vm)
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
        // Hack: Get the category Id
        _viewModel.Id = GetCategoryIdFromRoute();
        _viewModel.PageTitle = GetPageTitle(_viewModel.Id);
        await _viewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.Contents.Clear();
    }

    private string GetPageTitle(string pageTypeId)
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
            _ => "Evangélium, elmélkedés",
        };

        //Kedvencek "Kedvencek (%d)" getDataAccess().KedvencCount()
        //Gyónás előkészítő
        //Gyónás szövege
        //Gyónáshoz jegyzet
        //Gyónáshoz elmélkedés //gyónás olvasás megnyitásakor is a details oldalon
        //Gyónáshoz lelkitükör
        //Igenaptár
        //Keresztút
        //Webrádiók
        //Beállítások
        //"Menü sorrend/láthatóság"
        //Videók
        
        //Menü
        //Frissítés
    }

    private string GetCategoryIdFromRoute()
    {
        // Hack: As the shell can't define query parameters
        // in XAML, we have to parse the route. 
        // as a convention the last route section defines the category.
        // ugly but works for now :-(
        return Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault();
    }
}