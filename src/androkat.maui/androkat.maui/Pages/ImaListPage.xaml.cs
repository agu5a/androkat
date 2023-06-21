using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ImaListPage : ContentPage
{
    private int _stackCount = 0;
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
        ViewModel.PageTitle = "Imádságok";
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        await ViewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _stackCount = Application.Current.MainPage.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
    }
}