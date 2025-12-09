using androkat.maui.library.Abstraction;

namespace androkat.hu.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        // pages
        builder.Services.AddSingleton<ContentListPage>();
        builder.Services.AddSingleton<ImaListPage>();
        builder.Services.AddSingleton<FavoriteListPage>();
        builder.Services.AddSingleton<WebPage>();
        builder.Services.AddSingleton<ContactPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<IgeNaptarPage>();
        builder.Services.AddSingleton<KeresztutPage>();
        builder.Services.AddSingleton<VideoListPage>();
        builder.Services.AddSingleton<WebRadioPage>();
        builder.Services.AddSingleton<GyonasPage>();

        // pages that are navigated to
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<DetailPray>();
        builder.Services.AddTransient<DetailAudio>();
        builder.Services.AddTransient<DetailBook>();
        builder.Services.AddTransient<GyonasNotesPage>();
        builder.Services.AddTransient<GyonasPrayPage>();
        builder.Services.AddTransient<GyonasFinishPage>();
        builder.Services.AddTransient<GyonasMirrorPage>();

#if ANDROID
        builder.Services.AddSingleton<IDeviceDisplayService, Platforms.Android.Services.AndroidDeviceDisplayService>();
#elif IOS
        builder.Services.AddSingleton<IDeviceDisplayService, Platforms.iOS.Services.iOSDeviceDisplayService>();
#endif

        return builder;
    }
}
