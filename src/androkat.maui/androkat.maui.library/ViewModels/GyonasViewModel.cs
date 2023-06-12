using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class GyonasViewModel : ViewModelBase
{
    public GyonasViewModel()
    {
        Items = new ObservableRangeCollection<GyonasTile>();
    }

    [ObservableProperty]
    ObservableRangeCollection<GyonasTile> items;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<GyonasTile>
        {
            new GyonasTile("ELMÉLKEDÉS", "icon"),
            new GyonasTile("IMA", "icon"),
            new GyonasTile("LELKI TÜKÖR", "icon"),
            new GyonasTile("JEGYZET", "icon"),
            new GyonasTile("GYÓNÁS", "icon"),
            new GyonasTile("TÖRLÉS", "icon")
        };

        Items.ReplaceRange(s);
    }
}
