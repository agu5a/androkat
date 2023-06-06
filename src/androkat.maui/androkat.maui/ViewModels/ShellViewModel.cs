using androkat.hu.Models;
using androkat.hu.Pages;

namespace androkat.hu.ViewModels;

public class ShellViewModel : ViewModelBase
{
    public AppSection Discover { get; set; }
    public AppSection Subscriptions { get; set; }
    public AppSection Lists { get; set; }
    public AppSection ListenLater { get; set; }
    public AppSection Settings { get; set; }
    public AppSection ListenTogether { get; set; }

    public ShellViewModel()
    {
        Discover = new AppSection { Title = "HOME", Icon = "book", IconDark = "book", TargetType = typeof(DiscoverPage) };
        Subscriptions = new AppSection { Title = "Hallgass", Icon = "ic_radio_24dp", IconDark= "ic_radio_24dp", TargetType = typeof(DiscoverPage) }; //SubscriptionsPage
        ListenLater = new AppSection { Title = "Imádkozz", Icon = "candle", IconDark= "candle", TargetType = typeof(DiscoverPage) }; //ListenLaterPage
        ListenTogether = new AppSection { Title = "Kedvencek", Icon = "favorite", IconDark = "favorite",  TargetType = typeof(DiscoverPage) }; 
        Settings = new AppSection { Title = "Beállítások", Icon = "ic_settings_24dp", IconDark= "ic_settings_24dp", TargetType = typeof(SettingsPage) };
        //Settings = new AppSection() { Title = AppResource.Settings, Icon = "settings.png", IconDark="settings_dark.png", TargetType = typeof(SettingsPage) };
    }
}