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
            new WebUrl("Szent István Rádió", "http://online.szentistvanradio.hu:7000/adas", _browser),
            new WebUrl("Katolikus Rádió", "http://katolikusradio.hu:9000/live_hi.mp3", _browser),
            new WebUrl("Vatikáni Rádió", "https://media.vaticannews.va/media/audio/program/1900/ungherese_100623.mp3", _browser),
            new WebUrl("Ez az a nap Rádió", "https://www.radioking.com/play/ez-az-a-nap-radio", _browser),
            new WebUrl("Mária Rádió", "http://www.mariaradio.hu:8000/mr", _browser),
            new WebUrl("Mária Rádió Szerbia", "", _browser),
            new WebUrl("Mária Rádió Erdély", "", _browser),
            new WebUrl("Mária Rádió Szlovákia", "", _browser),
            new WebUrl("Sola Rádió", "", _browser)
        };

        Items.ReplaceRange(s);
    }
}
