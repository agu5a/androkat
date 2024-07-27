using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class WebPage : ContentPage
{
    private WebViewModel ViewModel => BindingContext as WebViewModel;

    public WebPage(WebViewModel viewModel)
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