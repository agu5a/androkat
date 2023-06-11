using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class IgeNaptarPage : ContentPage
{
    private IgeNaptarViewModel ViewModel => BindingContext as IgeNaptarViewModel;

    public IgeNaptarPage(IgeNaptarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        await ViewModel.InitializeAsync(e.NewDate.Day);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await ViewModel.InitializeAsync(100);
    }
}