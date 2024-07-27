using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class WebRadioPage : ContentPage
{
    private WebRadioViewModel ViewModel => BindingContext as WebRadioViewModel;

    public WebRadioPage(WebRadioViewModel viewModel)
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