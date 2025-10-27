using androkat.maui.library.Helpers;

namespace androkat.hu.Helpers;

public static class FontScaleHelper
{
    public static void ApplyFontScale()
    {
        try
        {
            // Check if Application.Current and Resources are available
            if (Application.Current?.Resources == null)
            {
                System.Diagnostics.Debug.WriteLine("FontScaleHelper: Application.Current.Resources not available yet");
                return;
            }

            var scale = Settings.GetFontScale();
            
            // Update all label styles with the new scale
            UpdateLabelStyles(scale);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error applying font scale: {ex.Message}");
        }
    }

    private static void UpdateLabelStyles(double scale)
    {
        var resources = Application.Current!.Resources;

        // Define the base font sizes for each style
        var styleDefinitions = new Dictionary<string, double>
        {
            { "H5LabelStyle", 32 },
            { "H6LabelStyle", 24 },
            { "H4LabelStyle", 36 },
            { "SH6LabelStyle", 18 },
            { "BodyXLLabelStyle", 20 },
            { "BodyLLabelStyle", 18 },
            { "BodyXLLabelNormalStyle", 20 },
            { "BodyLLabelNormalStyle", 18 },
            { "BodyMLabelStyle", 16 },
            { "BodySLabelStyle", 14 },
            { "LinkLLabelStyle", 20 }
        };

        foreach (var styleDefinition in styleDefinitions)
        {
            if (resources.TryGetValue(styleDefinition.Key, out var resource) && resource is Style style)
            {
                UpdateStyleFontSize(style, styleDefinition.Value * scale);
            }
        }
    }

    private static void UpdateStyleFontSize(Style style, double fontSize)
    {
        try
        {
            // Find and update the FontSize setter
            var fontSizeSetter = style.Setters.FirstOrDefault(s => s.Property.PropertyName == "FontSize");
            if (fontSizeSetter != null)
            {
                style.Setters.Remove(fontSizeSetter);
            }
            style.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = fontSize });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating style font size: {ex.Message}");
        }
    }
}
