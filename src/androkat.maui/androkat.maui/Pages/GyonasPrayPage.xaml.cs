using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasPrayPage : ContentPage
{
    private GyonasPrayViewModel ViewModel => (BindingContext as GyonasPrayViewModel)!;

    public GyonasPrayPage(GyonasPrayViewModel model)
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