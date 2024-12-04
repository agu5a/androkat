using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasMirrorPage : ContentPage
{
    private GyonasMirrorViewModel ViewModel => (BindingContext as GyonasMirrorViewModel)!;

    public GyonasMirrorPage(GyonasMirrorViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }
}