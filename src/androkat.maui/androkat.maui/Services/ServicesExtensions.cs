using androkat.hu.Data;
using androkat.hu.Helpers;

namespace androkat.hu.Services;

public static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(Browser.Default);

        string dbPath = FileAccessHelper.GetLocalFilePath("androkat.db3");
        builder.Services.AddSingleton<IRepository>(s => ActivatorUtilities.CreateInstance<Repository>(s, dbPath));
        
        builder.Services.AddSingleton<ISourceData, SourceDataMapper>();
        builder.Services.AddSingleton<IAndrokatService, AndrokatService>();
        return builder;
    }
}
