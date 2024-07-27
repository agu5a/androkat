using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class IgeNaptarPage : ContentPage
{
    private IgeNaptarViewModel ViewModel => BindingContext as IgeNaptarViewModel;

    public IgeNaptarPage(IgeNaptarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync(DateTime.Now);
    }

    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        await ViewModel.InitializeAsync(e.NewDate);
    }
}