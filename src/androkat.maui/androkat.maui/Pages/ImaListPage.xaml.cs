using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ImaListPage : ContentPage
{
    private ImaListViewModel ViewModel => BindingContext as ImaListViewModel;

    public ImaListPage(ImaListViewModel viewModel)
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
        await ViewModel.InitializeAsync();
        ViewModel.PageTitle = "Imádságok";
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}