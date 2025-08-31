using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using androkat.maui.library.Helpers;

namespace androkat.hu;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize
    | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Apply keep screen on setting
        ApplyKeepScreenOnSetting();
    }

    protected override void OnResume()
    {
        base.OnResume();

        // Re-apply keep screen on setting when activity resumes
        ApplyKeepScreenOnSetting();
    }

    private void ApplyKeepScreenOnSetting()
    {
        try
        {
            if (Settings.KeepScreenOn && Window != null)
            {
                Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            else if (Window != null)
            {
                Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error applying keep screen on setting: {ex.Message}");
        }
    }
}
