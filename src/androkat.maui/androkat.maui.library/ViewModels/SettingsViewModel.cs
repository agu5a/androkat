using androkat.maui.library.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace androkat.maui.library.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel()
    {
        isDarkModeEnabled = Settings.Theme == AppTheme.Dark;
    }

    [ObservableProperty]
    bool isDarkModeEnabled;

    [ObservableProperty]
    string maxOffline;

    partial void OnIsDarkModeEnabledChanged(bool value) => SettingsViewModel.ChangeUserAppTheme(value);

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
