using CommunityToolkit.Mvvm.ComponentModel;
using androkat.hu.Helpers;
using androkat.hu.Services;

namespace androkat.hu.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly PageService _pageService;

    public SettingsViewModel(PageService pageService)
    {
        _pageService = pageService;

        isDarkModeEnabled = Settings.Theme == AppTheme.Dark;
    }    

    [ObservableProperty]
    bool isDarkModeEnabled;

    partial void OnIsDarkModeEnabledChanged(bool value) =>
        ChangeUserAppTheme(value);    

    public string AppVersion => AppInfo.VersionString;

    void ChangeUserAppTheme(bool activateDarkMode)
    {
        Settings.Theme = activateDarkMode
            ? AppTheme.Dark
            : AppTheme.Light;

        if (activateDarkMode)
        {
            Preferences.Default.Set("themeSelection", "themeBlack1");
            //Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Preferences.Default.Set("themeSelection", "themeNormal");
            //Application.Current.UserAppTheme = AppTheme.Light;
        }

        TheTheme.SetTheme();
    }
}
