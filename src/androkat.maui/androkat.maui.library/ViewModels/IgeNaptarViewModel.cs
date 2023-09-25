using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;
using System.Globalization;

namespace androkat.maui.library.ViewModels;

public partial class IgeNaptarViewModel : ViewModelBase
{
    public IgeNaptarViewModel()
    {
        MinDate = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Local);
        MaxDate = new DateTime(DateTime.Now.Year, 12, 31, 0, 0, 0, DateTimeKind.Local);
        SelectedDate = DateTime.Now;
        Contents = [];
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
        var s = new List<IgeNaptarView> { new() { Html = "1" }, new() { Html = day.ToString() }, new() { Html = "3" } };
        Contents.ReplaceRange(s);
    }
}
