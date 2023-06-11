using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class IgeNaptarPage : ContentPage
{
    private IgeNaptarViewModel _viewModel => BindingContext as IgeNaptarViewModel;

    public IgeNaptarPage(IgeNaptarViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        await _viewModel.InitializeAsync(e.NewDate.Day);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _viewModel.InitializeAsync(100);
    }
}