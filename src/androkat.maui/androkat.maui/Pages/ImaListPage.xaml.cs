using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ImaListPage : ContentPage
{
    private int _stackCount = 0;
    private int _pageNumber = 1;
    private readonly int _pageSize = 10;
    private ImaListViewModel ViewModel => (BindingContext as ImaListViewModel)!;

    public ImaListPage(ImaListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ViewModel.PageTitle = "Imádságok";
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        else
        {
            // Visszajövünk DetailPage-ről, újra fetcheljük az adatokat hogy a read status frissüljön
            Task.Run(async () => await ViewModel.InitializeAsync(_pageNumber, _pageSize));
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
        await ViewModel.InitializeAsync(_pageNumber, _pageSize);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "<Pending>")]
    private async void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await ViewModel.InitializeAsync(++_pageNumber, _pageSize);
    }
}