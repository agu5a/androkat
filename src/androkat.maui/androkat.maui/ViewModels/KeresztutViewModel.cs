using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

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
        var s = new List<KeresztutView> { new KeresztutView { html = "1" }, new KeresztutView { html = "2" }, new KeresztutView { html = "3" } };
        Contents.ReplaceRange(s);
    }
}

public partial class KeresztutView : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    public string html { get; set; }
}
