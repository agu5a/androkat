using androkat.hu.Helpers;
using androkat.hu.Pages;
using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;

namespace androkat.hu;

public partial class App : Application
{
    Window? window;

    public App()
    {
        InitializeComponent();

        TheTheme.SetTheme();

        Routing.RegisterRoute(nameof(ContentListPage), typeof(ContentListPage));
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        Routing.RegisterRoute(nameof(DetailPray), typeof(DetailPray));
        Routing.RegisterRoute(nameof(DetailAudio), typeof(DetailAudio));
        Routing.RegisterRoute(nameof(DetailBook), typeof(DetailBook));
        Routing.RegisterRoute(nameof(GyonasNotesPage), typeof(GyonasNotesPage));
        Routing.RegisterRoute(nameof(GyonasPrayPage), typeof(GyonasPrayPage));
        Routing.RegisterRoute(nameof(GyonasFinishPage), typeof(GyonasFinishPage));
        Routing.RegisterRoute(nameof(GyonasMirrorPage), typeof(GyonasMirrorPage));
        Routing.RegisterRoute(nameof(WebViewPage), typeof(WebViewPage));

        // Apply keep screen on setting
        ApplyKeepScreenOnSetting();
    }

    private void ApplyKeepScreenOnSetting()
    {
        try
        {
            if (Settings.KeepScreenOn)
            {
                var deviceDisplayService = Handler?.MauiContext?.Services?.GetService<IDeviceDisplayService>();
                deviceDisplayService?.KeepScreenOn(true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error applying keep screen on setting: {ex.Message}");
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Apply font scale BEFORE creating the Shell to avoid timing issues
        FontScaleHelper.ApplyFontScale();

        window = new Window(new MobileShell());

        // Apply keep screen on setting when window is created
        window.Created += (s, e) =>
        {
            ApplyKeepScreenOnSetting();
        };

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