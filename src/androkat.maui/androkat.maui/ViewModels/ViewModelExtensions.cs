namespace androkat.hu.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ContentListViewModel>();
        builder.Services.AddSingleton<DiscoverViewModel>();
        builder.Services.AddSingleton<NavigationViewModel>();


        return builder;
    }
}
