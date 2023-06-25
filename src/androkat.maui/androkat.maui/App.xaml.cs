using androkat.hu.Helpers;
using androkat.hu.Pages;
using Application = Microsoft.Maui.Controls.Application;

namespace androkat.hu;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        TheTheme.SetTheme();

        MainPage = new MobileShell();

        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
    }

    Window window;
    protected override Window CreateWindow(IActivationState activationState)
    {
        window = base.CreateWindow(activationState);
        return window;
    }

    private void Window_SizeChanged(object sender, EventArgs e)
    {
        if (window is null)
            return;

        if (window.Width < 1200)
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        else
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
    }
}