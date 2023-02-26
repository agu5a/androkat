using androkat.hu.ViewModels;

namespace androkat.hu.Pages;

public partial class ContactPage : ContentPage
{
    public ContactPage(ContactViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}