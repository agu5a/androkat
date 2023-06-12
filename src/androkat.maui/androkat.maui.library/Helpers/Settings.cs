namespace androkat.maui.library.Helpers;

public static class Settings
{
    public static AppTheme Theme
    {
        get
        {
            if (!Preferences.ContainsKey(nameof(Theme)))
                return AppTheme.Light;

            return Enum.Parse<AppTheme>(Preferences.Get(nameof(Theme), Enum.GetName(AppTheme.Light)));
        }
        set => Preferences.Set(nameof(Theme), value.ToString());
    }   
}