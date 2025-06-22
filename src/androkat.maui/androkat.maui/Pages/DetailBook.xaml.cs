using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailBook
{
    private BookDetailViewModel ViewModel => (BindingContext as BookDetailViewModel)!;

    public DetailBook(BookDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeBookAsync();
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }
}