using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailBook
{
    private DetailViewModel ViewModel => (BindingContext as DetailViewModel)!;

    public DetailBook(DetailViewModel viewModel)
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