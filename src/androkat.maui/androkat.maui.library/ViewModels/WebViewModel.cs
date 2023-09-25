using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class WebViewModel : ViewModelBase
{
    private readonly IBrowser _browser;

    public WebViewModel(IBrowser browser)
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
            new("Katekizmus", ConsValues.Katekizmus, _browser),
            new("E-Biblia", ConsValues.EBiblia, _browser),
            new("Zsolozsma", ConsValues.Zsolozsma, _browser),
            new("MiseRend", ConsValues.MiseRend, _browser),
            new("Megszentelt tér", ConsValues.MegszenteltTer, _browser),
            new("Bonum TV élő", ConsValues.BonumTv, _browser),
            new("liturgia.tv", ConsValues.LiturgiaTv, _browser)
        };

        Items.ReplaceRange(s);
    }
}
