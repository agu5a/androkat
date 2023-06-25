using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

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
            new WebUrl("Katekizmus", ConsValues.Katekizmus, _browser),
            new WebUrl("E-Biblia", ConsValues.EBiblia, _browser),
            new WebUrl("Zsolozsma", ConsValues.Zsolozsma, _browser),
            new WebUrl("MiseRend", ConsValues.MiseRend, _browser),
            new WebUrl("Megszentelt tér", ConsValues.MegszenteltTer, _browser),
            new WebUrl("Bonum TV élő", ConsValues.BonumTv, _browser),
            new WebUrl("liturgia.tv", ConsValues.LiturgiaTv, _browser)
        };

        Items.ReplaceRange(s);
    }
}
