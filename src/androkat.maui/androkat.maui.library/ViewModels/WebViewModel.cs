using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class WebViewModel : ViewModelBase
{
    public WebViewModel()
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
            new("Katekizmus", ConsValues.Katekizmus),
            new("E-Biblia", ConsValues.EBiblia),
            new("Zsolozsma", ConsValues.Zsolozsma),
            new("MiseRend", ConsValues.MiseRend),
            new("Megszentelt tér", ConsValues.MegszenteltTer),
            new("Bonum TV élő", ConsValues.BonumTv),
            new("liturgia.tv", ConsValues.LiturgiaTv)
        };

        Items.ReplaceRange(s);
    }
}
