using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasFinishPage : ContentPage
{
    private GyonasFinishViewModel ViewModel => (BindingContext as GyonasFinishViewModel)!;

    public GyonasFinishPage(GyonasFinishViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        await ViewModel.InitializeAsync();
    }
}