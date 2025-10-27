using androkat.maui.library.Helpers;

namespace androkat.hu.Helpers;

public static class HtmlHelper
{
    public static string WrapHtmlWithFontScale(string htmlContent)
    {
        var scale = Settings.GetFontScale();
        var baseFontSize = 16; // Base font size in pixels
        var scaledFontSize = baseFontSize * scale;

        // Check if the content already has HTML wrapper
        if (htmlContent.Contains("<html>") || htmlContent.Contains("<!DOCTYPE html>"))
        {
            // If it already has HTML wrapper, inject the font size into existing style
            return InjectFontSizeIntoHtml(htmlContent, scaledFontSize);
        }

        // Wrap content with HTML and apply font scale
        return $@"<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            margin: 0;
            padding: 16px;
            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            font-size: {scaledFontSize}px;
            line-height: 1.5;
        }}
    </style>
</head>
<body>{htmlContent}</body>
</html>";
    }

    private static string InjectFontSizeIntoHtml(string htmlContent, double fontSize)
    {
        // Find existing body style and update font-size, or add it if not present
        if (htmlContent.Contains("font-size:"))
        {
            // Replace existing font-size with scaled value
            var regex = new System.Text.RegularExpressions.Regex(@"font-size:\s*\d+(\.\d+)?\s*px");
            return regex.Replace(htmlContent, $"font-size:{fontSize}px");
        }
        else if (htmlContent.Contains("body style="))
        {
            // Add font-size to existing body style
            return htmlContent.Replace("body style='", $"body style='font-size:{fontSize}px;");
        }
        else if (htmlContent.Contains("<body>"))
        {
            // Add style attribute to body tag
            return htmlContent.Replace("<body>", $"<body style='font-size:{fontSize}px;'>");
        }

        // If no body tag found, return original content
        return htmlContent;
    }

    public static string GetScaledFontSizeStyle()
    {
        var scale = Settings.GetFontScale();
        var baseFontSize = 16;
        var scaledFontSize = baseFontSize * scale;
        return $"font-size:{scaledFontSize}px;";
    }
}
