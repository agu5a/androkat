using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class KeresztutViewModel : ViewModelBase
{
    private readonly IResourceData _resourceData;

    public KeresztutViewModel(IResourceData resourceData)
    {
        Contents = [];
        _resourceData = resourceData;
    }

    [ObservableProperty]
    ObservableRangeCollection<KeresztutView> contents;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);

        try
        {
            var htmlContents = new List<KeresztutView>
            {
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("bevezeto.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas1.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas2.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas3.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas4.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas5.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas6.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas7.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas8.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas9.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas10.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas11.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas12.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas13.html")) },
                new() { Html = await ProcessHtmlContent(await _resourceData.GetResourceAsString("allomas14.html")) }
            };
            Contents.ReplaceRange(htmlContents);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** KeresztutViewModel EXCEPTION! {ex}");
        }
    }

    private static async Task<string> ProcessHtmlContent(string htmlContent)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return htmlContent;

        try
        {
            // Replace image references with base64 encoded images
            for (int i = 1; i <= 14; i++)
            {
                string imgSrc = $"{i}.png";
                if (htmlContent.Contains(imgSrc))
                {
                    var base64Image = await GetImageAsBase64(imgSrc);
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        htmlContent = htmlContent.Replace($"src=\"{imgSrc}\"", $"src=\"data:image/png;base64,{base64Image}\"");
                    }
                }
            }

            // Get the scaled font size
            var scale = Settings.GetFontScale();
            var baseFontSize = 16;
            var scaledFontSize = baseFontSize * scale;

            // Wrap content in a complete HTML structure with CSS for better styling
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no'>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
            margin: 16px;
            padding: 0;
            line-height: 1.6;
            color: #333;
            background-color: #fff;
            font-size: {scaledFontSize}px;
        }}
        h3 {{
            color: #2c3e50;
            text-align: center;
            margin-bottom: 20px;
        }}
        img {{
            max-width: 100%;
            height: auto;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }}
        div {{
            margin-bottom: 16px;
            text-align: justify;
        }}
        i {{
            font-style: italic;
            color: #555;
        }}
        .center {{
            text-align: center;
        }}
    </style>
</head>
<body>
    {htmlContent}
</body>
</html>";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** ProcessHtmlContent EXCEPTION! {ex}");
            return htmlContent;
        }
    }

    private static async Task<string> GetImageAsBase64(string imageName)
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(imageName);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetImageAsBase64 EXCEPTION for {imageName}! {ex}");
            return string.Empty;
        }
    }
}
