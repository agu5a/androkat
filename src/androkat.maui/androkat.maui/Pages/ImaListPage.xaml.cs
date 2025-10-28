using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ImaListPage
{
    private int _stackCount;
    private ImaListViewModel ViewModel => (BindingContext as ImaListViewModel)!;

    public ImaListPage(ImaListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Reset pagination when navigating to the page (but not when returning from detail page)
        if (_stackCount != 2)
        {
            // Reset category to "Összes" (first item in Categories)
            ViewModel.ResetToDefaultCategory();
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
        await ViewModel.InitializeAsync();
    }
}