using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasNotesPage : ContentPage
{
    private GyonasNotesViewModel ViewModel => (BindingContext as GyonasNotesViewModel)!;

    public GyonasNotesPage(GyonasNotesViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }

    private void editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.IsSaved = false;
        ViewModel.JegyzetTextChanged();
    }
}