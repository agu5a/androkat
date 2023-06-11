using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class IgeNaptarViewModel : ViewModelBase
{
    public IgeNaptarViewModel()
    {
        MinDate = new DateTime(DateTime.Now.Year, 1, 1);
        MaxDate = new DateTime(DateTime.Now.Year, 12, 31);
        SelectedDate = DateTime.Now;
        Contents = new ObservableRangeCollection<IgeNaptarView>();
    }

    [ObservableProperty]
    DateTime minDate;

    [ObservableProperty]
    DateTime maxDate;

    [ObservableProperty]
    DateTime selectedDate;

    [ObservableProperty]
    ObservableRangeCollection<IgeNaptarView> contents;

    public async Task InitializeAsync(int day)
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<IgeNaptarView> { new IgeNaptarView { html = "1" }, new IgeNaptarView { html = day.ToString() }, new IgeNaptarView { html = "3" } };
        Contents.ReplaceRange(s);
    }
}

public partial class IgeNaptarView : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    public string html { get; set; }
}
