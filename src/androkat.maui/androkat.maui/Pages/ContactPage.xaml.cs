using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ContactPage : ContentPage
{
    public ContactPage(ContactViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}