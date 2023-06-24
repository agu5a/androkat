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

    partial void OnIsDarkModeEnabledChanged(bool value) => ChangeUserAppTheme(value);

    public string AppVersion => AppInfo.VersionString;

    void ChangeUserAppTheme(bool activateDarkMode)
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
