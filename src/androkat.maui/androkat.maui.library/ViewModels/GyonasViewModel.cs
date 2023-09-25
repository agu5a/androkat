using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class GyonasViewModel : ViewModelBase
{
    public GyonasViewModel()
    {
        Items = [];
    }

    [ObservableProperty]
    ObservableRangeCollection<GyonasTile> items;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<GyonasTile>
        {
            new("ELMÉLKEDÉS", "icon"),
            new("IMA", "icon"),
            new("LELKI TÜKÖR", "icon"),
            new("JEGYZET", "icon"),
            new("GYÓNÁS", "icon"),
            new("TÖRLÉS", "icon")
        };

        Items.ReplaceRange(s);
    }
}
