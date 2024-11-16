using androkat.maui.library.Helpers;

namespace androkat.hu.Helpers;

public static class TheTheme
{
    public static void SetTheme()
    {
        Application.Current!.UserAppTheme = Settings.Theme;
    }
}
