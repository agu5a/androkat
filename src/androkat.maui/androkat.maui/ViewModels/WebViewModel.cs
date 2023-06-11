using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class WebViewModel : ViewModelBase
{
    private readonly IBrowser _browser;

    public WebViewModel(IBrowser browser)
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
            new WebUrl("Katekizmus", "https://archiv.katolikus.hu/kek/", _browser),
            new WebUrl("E-Biblia", "http://szentiras.hu/", _browser),
            new WebUrl("Zsolozsma", "http://zsolozsma.katolikus.hu/", _browser),
            new WebUrl("MiseRend", "http://miserend.hu/", _browser),
            new WebUrl("Megszentelt tér", "http://www.szentter.com/", _browser),
            new WebUrl("Bonum TV élő", "https://katolikus.tv/elo-adas/", _browser),
            new WebUrl("liturgia.tv", "https://liturgia.tv/", _browser)
        };

        Items.ReplaceRange(s);
    }
}
