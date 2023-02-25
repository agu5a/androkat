using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class ShowDetailPage : ContentPage
{
    private ShowDetailViewModel viewModel => BindingContext as ShowDetailViewModel;

    public ShowDetailPage(ShowDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}