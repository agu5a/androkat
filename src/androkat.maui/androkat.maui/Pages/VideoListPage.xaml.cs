using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class VideoListPage : ContentPage
{
    private VideoListViewModel ViewModel => BindingContext as VideoListViewModel;

    public VideoListPage(VideoListViewModel viewModel)
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
        await ViewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }
}