namespace androkat.hu.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ContentListViewModel>();
        builder.Services.AddSingleton<ImaListViewModel>();
        builder.Services.AddSingleton<FavoriteListViewModel>();
        builder.Services.AddSingleton<WebViewModel>();
        builder.Services.AddSingleton<ContactViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddTransient<ShowDetailViewModel>();
        builder.Services.AddTransient<IgeNaptarViewModel>();

        return builder;
    }
}
