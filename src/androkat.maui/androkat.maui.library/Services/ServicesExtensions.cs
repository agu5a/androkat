using androkat.hu.Data;
using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Helpers;

namespace androkat.maui.library.Services;

public static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(Browser.Default);

        string dbPath = FileAccessHelper.GetLocalFilePath("androkat.db3");
        builder.Services.AddSingleton<IRepository>(s => ActivatorUtilities.CreateInstance<Repository>(s, dbPath));
        builder.Services.AddSingleton<IDownloadService, DownloadService>();
        builder.Services.AddSingleton<ISourceData, SourceDataMapper>();
        builder.Services.AddSingleton<IResourceData, ResourceDataService>();
        builder.Services.AddSingleton<IHelperSharedPreferences, HelperSharedPreferences>();
        builder.Services.AddSingleton<IAndrokatService, AndrokatService>();
        builder.Services.AddSingleton<IPageService, PageService>();
        return builder;
    }
}
