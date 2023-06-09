using androkat.maui.library.Helpers;

namespace androkat.hu.Helpers;

public static class TheTheme
{
    public static void SetTheme()
    {
        switch (Settings.Theme)
        {
            default:
            case AppTheme.Light:
                Application.Current.UserAppTheme = AppTheme.Light;
                break;
            case AppTheme.Dark:
                Application.Current.UserAppTheme = AppTheme.Dark;
                break;

        }
    }
}
