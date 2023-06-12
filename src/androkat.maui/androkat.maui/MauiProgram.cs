using androkat.hu.Pages;
using androkat.maui.library.Services;
using androkat.maui.library.ViewModels;
using MonkeyCache.FileStore;

namespace androkat.hu;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureEssentials()
            .ConfigureServices()
            .ConfigurePages()
            .ConfigureViewModels()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        Barrel.ApplicationId = "androkat.hu";

        return builder.Build();
    }
}