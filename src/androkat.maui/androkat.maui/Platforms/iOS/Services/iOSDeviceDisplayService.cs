using androkat.maui.library.Abstraction;
using UIKit;

namespace androkat.hu.Platforms.iOS.Services;

public class iOSDeviceDisplayService : IDeviceDisplayService
{
    public void KeepScreenOn(bool keepOn)
    {
        try
        {
            UIApplication.SharedApplication.IdleTimerDisabled = keepOn;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting keep screen on: {ex.Message}");
        }
    }
}
