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
        selectedFontSizeIndex = Settings.FontSize;
    }

    [ObservableProperty]
    bool isDarkModeEnabled;

    [ObservableProperty]
    bool isKeepScreenOnEnabled;

    [ObservableProperty]
    string maxOffline;

    [ObservableProperty]
    int selectedFontSizeIndex;

    public List<string> FontSizeOptions { get; } = new List<string>
    {
        "Normál",
        "Nagyobb",
        "Még nagyobb",
        "Legnagyobb"
    };

    partial void OnIsDarkModeEnabledChanged(bool value) => SettingsViewModel.ChangeUserAppTheme(value);

    partial void OnIsKeepScreenOnEnabledChanged(bool value)
    {
        Settings.KeepScreenOn = value;
        _deviceDisplayService?.KeepScreenOn(value);
    }

    partial void OnSelectedFontSizeIndexChanged(int value)
    {
        Settings.FontSize = value;
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
