using Android.Views;
using androkat.maui.library.Abstraction;

namespace androkat.hu.Platforms.Android.Services;

public class AndroidDeviceDisplayService : IDeviceDisplayService
{
    public void KeepScreenOn(bool keepOn)
    {
        try
        {
            var activity = Platform.CurrentActivity ?? Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            if (activity?.Window != null)
            {
                if (keepOn)
                {
                    activity.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                }
                else
                {
                    activity.Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting keep screen on: {ex.Message}");
        }
    }
}
