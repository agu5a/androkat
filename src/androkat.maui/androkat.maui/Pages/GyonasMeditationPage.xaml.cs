using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasMeditationPage : ContentPage
{
    //private GyonasMeditationViewModel ViewModel => (BindingContext as GyonasMeditationViewModel)!

    public GyonasMeditationPage(GyonasMeditationViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}