using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ImaListPage
{
    private int _stackCount;
    private int _pageNumber = 1;
    private const int PageSize = 10;
    private bool _isInitialized = false;
    private ImaListViewModel ViewModel => (BindingContext as ImaListViewModel)!;

    public ImaListPage(ImaListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ViewModel.PageTitle = "Imádságok";

        // Reset pagination when navigating to the page (but not when returning from detail page)
        if (_stackCount != 2)
        {
            // Coming from somewhere else, reset everything
            ResetPagination();
            ViewModel.Contents.Clear();
            _isInitialized = false;
        }
        else
        {
            // Returning from DetailPage, refresh current data to update read status
            // but don't reset pagination
            Task.Run(async () => await RefreshCurrentData());
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

        // Only load initial data if not already initialized
        if (!_isInitialized)
        {
            await LoadInitialData();
        }
    }

    private async Task LoadInitialData()
    {
        ResetPagination();
        ViewModel.Contents.Clear();
        await ViewModel.InitializeAsync(_pageNumber, PageSize);
        _isInitialized = true;
    }

    public async Task RefreshForCategoryChange()
    {
        ResetPagination();
        ViewModel.Contents.Clear();
        await ViewModel.FetchAsync(_pageNumber, PageSize);
    }

    private async Task RefreshCurrentData()
    {
        // Clear and reload all current pages to refresh read status
        ViewModel.Contents.Clear();
        var currentMaxPage = _pageNumber;
        ResetPagination();

        // Reload all pages that were previously loaded
        for (int page = 1; page <= currentMaxPage; page++)
        {
            await ViewModel.FetchAsync(page, PageSize);
        }
        _pageNumber = currentMaxPage;
    }

    private void ResetPagination()
    {
        _pageNumber = 1;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "<Pending>")]
    private async void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        // Only load more if we're not filtered by category or if we have more items
        if (ViewModel.SelectedCategory?.Id == -1 || ViewModel.SelectedCategory?.Id == null)
        {
            // Load next page for infinite scroll
            _pageNumber++;
            await ViewModel.FetchAsync(_pageNumber, PageSize);
        }
    }
}