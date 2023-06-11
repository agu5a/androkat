using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class WebRadioPage : ContentPage
{
    private WebRadioViewModel ViewModel => BindingContext as WebRadioViewModel;

    public WebRadioPage(WebRadioViewModel viewModel)
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