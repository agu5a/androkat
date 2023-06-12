using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class KeresztutViewModel : ViewModelBase
{
    public KeresztutViewModel()
    {
        Contents = new ObservableRangeCollection<KeresztutView>();
    }

    [ObservableProperty]
    ObservableRangeCollection<KeresztutView> contents;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<KeresztutView> { new KeresztutView { Html = "1" }, new KeresztutView { Html = "2" }, new KeresztutView { Html = "3" } };
        Contents.ReplaceRange(s);
    }
}
