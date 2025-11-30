using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class IgeNaptarPage : ContentPage
{
    private IgeNaptarViewModel ViewModel => (BindingContext as IgeNaptarViewModel)!;

    public IgeNaptarPage(IgeNaptarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(DateTime.Now);
        base.OnAppearing();
        ViewModel.SetPosition(DateTime.Now);
        var myCarouselView = this.FindByName<CarouselView>("MyCarouselView");
        myCarouselView.ScrollTo(ViewModel.Position);
    }

    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        await ViewModel.InitializeAsync(e.NewDate ?? DateTime.Now);
        ViewModel.SetPosition(e.NewDate ?? DateTime.Now);
        var myCarouselView = this.FindByName<CarouselView>("MyCarouselView");
        myCarouselView.ScrollTo(ViewModel.Position);
    }
}