using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class WebRadioViewModel : ViewModelBase
{
    public WebRadioViewModel()
    {
        Items = [];
    }

    [ObservableProperty]
    ObservableRangeCollection<WebUrl> items;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<WebUrl>
        {
            new("Szent István Rádió", ConsValues.Szentistvanradio),
            new("Katolikus Rádió", ConsValues.Katolikusradio),
            new("Vatikáni Rádió", ConsValues.Vaticannews),
            new("Ez az a nap Rádió", ConsValues.EzAzANap),
            new("Mária Rádió", ConsValues.Mariaradio),
            new("Mária Rádió Szerbia", ""),
            new("Mária Rádió Erdély", ""),
            new("Mária Rádió Szlovákia", ""),
            new("Sola Rádió", "")
        };

        Items.ReplaceRange(s);
    }
}
