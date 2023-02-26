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

        await _viewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.Contents.Clear();
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