using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class KeresztutPage : ContentPage
{
    private KeresztutViewModel _viewModel => BindingContext as KeresztutViewModel;

    public KeresztutPage(KeresztutViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _viewModel.InitializeAsync();
    }
}