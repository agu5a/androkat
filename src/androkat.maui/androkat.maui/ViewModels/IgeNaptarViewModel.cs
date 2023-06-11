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
        var s = new List<IgeNaptarView> { new IgeNaptarView { Html = "1" }, new IgeNaptarView { Html = day.ToString() }, new IgeNaptarView { Html = "3" } };
        Contents.ReplaceRange(s);
    }
}
