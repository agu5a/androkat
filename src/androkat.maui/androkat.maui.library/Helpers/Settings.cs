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

    public static bool KeepScreenOn
    {
        get => Preferences.Get(nameof(KeepScreenOn), false);
        set => Preferences.Set(nameof(KeepScreenOn), value);
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
        var enabledSources = new List<string>();

        foreach (var option in availableOptions)
        {
            var isEnabled = IsSourceEnabled(option.Key);
            System.Diagnostics.Debug.WriteLine($"Source '{option.Key}' ({option.DisplayName}) enabled: {isEnabled}");
            if (isEnabled)
            {
                enabledSources.Add(option.Key);
            }
        }

        System.Diagnostics.Debug.WriteLine($"Settings.GetEnabledSources returning: [{string.Join(", ", enabledSources)}] from {availableOptions.Count} available options");

        // If no sources are enabled but we have available options, it suggests a platform issue
        // In this case, return all available sources as the default behavior
        if (enabledSources.Count == 0 && availableOptions.Count > 0)
        {
            System.Diagnostics.Debug.WriteLine("No sources enabled but options available - returning all sources as default");
            enabledSources = availableOptions.Select(option => option.Key).ToList();
        }

        return enabledSources;
    }
}