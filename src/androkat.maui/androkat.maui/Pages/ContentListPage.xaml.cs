using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class ContentListPage : ContentPage
{
    private ContentListViewModel viewModel => BindingContext as ContentListViewModel;

    public ContentListPage(ContentListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;    
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //player.OnAppearing();
        await viewModel.InitializeAsync();
    }


    protected override void OnDisappearing()
    {
        //player.OnDisappearing();
        base.OnDisappearing();
        viewModel.Contents.Clear();
    }
}