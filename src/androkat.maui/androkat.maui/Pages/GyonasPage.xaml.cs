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

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await ViewModel.InitializeAsync();
        activityIndicator.IsRunning = false;
        activityIndicator.IsVisible = false;
    }
}