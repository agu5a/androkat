using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class MobileShell
{
    public MobileShell()
    {
        InitializeComponent();

        BindingContext = new ShellViewModel();
    }
}