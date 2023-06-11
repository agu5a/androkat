using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class ShowDetailPage : ContentPage
{
    private ShowDetailViewModel ViewModel => BindingContext as ShowDetailViewModel;

    public ShowDetailPage(ShowDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }
}