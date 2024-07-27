using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasPage : ContentPage
{
    private GyonasViewModel ViewModel => BindingContext as GyonasViewModel;

    public GyonasPage(GyonasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        activityIndicator.IsRunning = false;
        activityIndicator.IsVisible = false;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }
}