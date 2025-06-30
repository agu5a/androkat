using androkat.maui.library.Models;

namespace androkat.maui.library.Helpers;

public static class Settings
{
    public static DateTime LastVersionCheck { get; set; } = DateTime.MinValue;
    public static DateTime LastUpdate { get; set; } = DateTime.MinValue;

    public static AppTheme Theme
    {
        get
        {
            if (!Preferences.ContainsKey(nameof(Theme)))
                return AppTheme.Light;

            return Enum.Parse<AppTheme>(Preferences.Get(nameof(Theme), Enum.GetName(AppTheme.Light))!);
        }
        set => Preferences.Set(nameof(Theme), value.ToString());
    }

    public static bool ShowVisited
    {
        get => Preferences.Get(nameof(ShowVisited), true);
        set => Preferences.Set(nameof(ShowVisited), value);
    }

    public static bool IsSourceEnabled(string sourceKey)
    {
        return Preferences.Get($"source_{sourceKey}", true);
    }

    public static void SetSourceEnabled(string sourceKey, bool enabled)
    {
        Preferences.Set($"source_{sourceKey}", enabled);
    }

    public static List<string> GetEnabledSources(List<FilterOption> availableOptions)
    {
        var enabledSources = availableOptions
            .Where(option => IsSourceEnabled(option.Key))
            .Select(option => option.Key)
            .ToList();

        System.Diagnostics.Debug.WriteLine($"Settings.GetEnabledSources returning: [{string.Join(", ", enabledSources)}]");
        return enabledSources;
    }
}