using androkat.hu.Helpers;
using androkat.hu.Pages;

namespace androkat.hu;

public partial class App : Application
{
    Window? window;

    public App()
    {
        InitializeComponent();

        TheTheme.SetTheme();

        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        Routing.RegisterRoute(nameof(GyonasMeditationPage), typeof(GyonasMeditationPage));
        Routing.RegisterRoute(nameof(GyonasPrayPage), typeof(GyonasPrayPage));
        Routing.RegisterRoute(nameof(GyonasFinishPage), typeof(GyonasFinishPage));
    }
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        window = new Window(new MobileShell());
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