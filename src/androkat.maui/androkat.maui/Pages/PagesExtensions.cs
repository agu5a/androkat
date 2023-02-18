namespace androkat.hu.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ContentListPage>();
        builder.Services.AddSingleton<DiscoverPage>();

        builder.Services.AddTransient<ShowDetailPage>();

        return builder;
    }
}
