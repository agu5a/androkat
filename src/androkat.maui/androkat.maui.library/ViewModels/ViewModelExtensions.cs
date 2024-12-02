namespace androkat.maui.library.ViewModels;

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
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<IgeNaptarViewModel>();
        builder.Services.AddTransient<KeresztutViewModel>();
        builder.Services.AddTransient<VideoListViewModel>();
        builder.Services.AddTransient<WebRadioViewModel>();
        builder.Services.AddTransient<GyonasViewModel>();
        builder.Services.AddTransient<GyonasNotesViewModel>();
        builder.Services.AddTransient<GyonasPrayViewModel>();
        builder.Services.AddTransient<GyonasFinishViewModel>();

        return builder;
    }
}
