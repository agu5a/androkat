using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace androkat.maui.library.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly IDeviceDisplayService _deviceDisplayService;

    public SettingsViewModel(IDeviceDisplayService deviceDisplayService)
    {
        _deviceDisplayService = deviceDisplayService;
        isDarkModeEnabled = Settings.Theme == AppTheme.Dark;
        isKeepScreenOnEnabled = Settings.KeepScreenOn;
    }

    [ObservableProperty]
    bool isDarkModeEnabled;

    [ObservableProperty]
    bool isKeepScreenOnEnabled;

    [ObservableProperty]
    string maxOffline;

    partial void OnIsDarkModeEnabledChanged(bool value) => SettingsViewModel.ChangeUserAppTheme(value);

    partial void OnIsKeepScreenOnEnabledChanged(bool value)
    {
        Settings.KeepScreenOn = value;
        _deviceDisplayService?.KeepScreenOn(value);
    }

    public static string AppVersion => AppInfo.VersionString;

    static void ChangeUserAppTheme(bool activateDarkMode)
    {
        Settings.Theme = activateDarkMode
            ? AppTheme.Dark
            : AppTheme.Light;

        if (activateDarkMode)
        {
            Preferences.Default.Set("themeSelection", "themeBlack1");
        }
        else
        {
            Preferences.Default.Set("themeSelection", "themeNormal");
        }
    }
}
