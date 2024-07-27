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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }
}