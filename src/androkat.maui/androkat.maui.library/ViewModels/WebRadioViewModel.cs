using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class WebRadioViewModel : ViewModelBase
{
    private readonly IBrowser _browser;

    public WebRadioViewModel(IBrowser browser)
    {
        Items = new ObservableRangeCollection<WebUrl>();
        _browser = browser;
    }

    [ObservableProperty]
    ObservableRangeCollection<WebUrl> items;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<WebUrl>
        {
            new WebUrl("Szent István Rádió", ConsValues.Szentistvanradio, _browser),
            new WebUrl("Katolikus Rádió", ConsValues.Katolikusradio, _browser),
            new WebUrl("Vatikáni Rádió", ConsValues.Vaticannews, _browser),
            new WebUrl("Ez az a nap Rádió", ConsValues.EzAzANap, _browser),
            new WebUrl("Mária Rádió", ConsValues.Mariaradio, _browser),
            new WebUrl("Mária Rádió Szerbia", "", _browser),
            new WebUrl("Mária Rádió Erdély", "", _browser),
            new WebUrl("Mária Rádió Szlovákia", "", _browser),
            new WebUrl("Sola Rádió", "", _browser)
        };

        Items.ReplaceRange(s);
    }
}
