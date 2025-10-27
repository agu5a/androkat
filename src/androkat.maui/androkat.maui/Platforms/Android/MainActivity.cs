using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using androkat.maui.library.Helpers;
using AndroidX.Core.View;

namespace androkat.hu;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize
    | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set status bar color to match the app theme (green)
        SetStatusBarColor();

        // Apply keep screen on setting
        ApplyKeepScreenOnSetting();
    }

    private void SetStatusBarColor()
    {
        if (Window != null && Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            var statusBarColor = Android.Graphics.Color.ParseColor("#00796B");

#pragma warning disable CA1416 // Validate platform compatibility
            if (Build.VERSION.SdkInt >= BuildVersionCodes.VanillaIceCream) // API 35+
            {
                // Use the new approach for Android 15+
                // The window background color is used for status bar by default
                // with edge-to-edge mode
                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);

                // To actually set the color, you'd typically use theme attributes or
                // apply the color to your root layout instead
            }
            else
            {
                // Use the deprecated method for older versions
                Window.SetStatusBarColor(statusBarColor);
            }

            // Set light status bar icons (works on both old and new APIs)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R) // API 30+
            {
                Window?.InsetsController?.SetSystemBarsAppearance(
                    (int)Android.Views.WindowInsetsControllerAppearance.LightStatusBars,
                    (int)Android.Views.WindowInsetsControllerAppearance.LightStatusBars
                );
            }
#pragma warning restore CA1416
        }
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
