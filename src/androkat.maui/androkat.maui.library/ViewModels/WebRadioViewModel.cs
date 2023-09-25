using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class WebRadioViewModel : ViewModelBase
{
    private readonly IBrowser _browser;

    public WebRadioViewModel(IBrowser browser)
    {
        Items = [];
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
            new("Szent István Rádió", ConsValues.Szentistvanradio, _browser),
            new("Katolikus Rádió", ConsValues.Katolikusradio, _browser),
            new("Vatikáni Rádió", ConsValues.Vaticannews, _browser),
            new("Ez az a nap Rádió", ConsValues.EzAzANap, _browser),
            new("Mária Rádió", ConsValues.Mariaradio, _browser),
            new("Mária Rádió Szerbia", "", _browser),
            new("Mária Rádió Erdély", "", _browser),
            new("Mária Rádió Szlovákia", "", _browser),
            new("Sola Rádió", "", _browser)
        };

        Items.ReplaceRange(s);
    }
}
