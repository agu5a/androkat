using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailAudio
{
    private DetailViewModel ViewModel => (BindingContext as DetailViewModel)!;

    public DetailAudio(DetailViewModel viewModel)
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