namespace androkat.hu.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        // main tabs of the app
        builder.Services.AddSingleton<ContentListPage>();
        builder.Services.AddSingleton<DiscoverPage>();
        builder.Services.AddSingleton<WebPage>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddTransient<ShowDetailPage>();

        return builder;
    }
}
