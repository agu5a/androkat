using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;
using System.Text.Json;

namespace androkat.maui.library.ViewModels;

public partial class IgeNaptarViewModel : ViewModelBase
{
    private readonly IResourceData _resourceData;

    public IgeNaptarViewModel(IResourceData resourceData)
    {
        MinDate = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Local);
        MaxDate = new DateTime(DateTime.Now.Year, 12, 31, 0, 0, 0, DateTimeKind.Local);
        SelectedDate = DateTime.Now;
        Contents = [];
        _resourceData = resourceData;
    }

    [ObservableProperty]
    DateTime minDate;

    [ObservableProperty]
    DateTime maxDate;

    [ObservableProperty]
    DateTime selectedDate;

    [ObservableProperty]
    ObservableRangeCollection<IgeNaptarView> contents;

    [ObservableProperty]
    int position;

    [ObservableProperty]
    IgeNaptarView currentItem;

    public async Task InitializeAsync(DateTime day)
    {
        //Delay on first load until window loads
        await Task.Delay(1000);

        var igenaptar = await _resourceData.GetResourceAsString("igenaptar3.json");
        var dayContents = JsonSerializer.Deserialize<Dictionary<string, string>>(igenaptar);

        var s = StartCheck(day, dayContents);
        Contents.ReplaceRange(s);
    }

    public void SetPosition(DateTime day)
    {
        // Always select the first item to work around MAUI CarouselView positioning issues
        if (Contents.Count > 0)
        {
            Position = 0;
            CurrentItem = Contents[0];
        }
    }

    private static List<IgeNaptarView> StartCheck(DateTime date, Dictionary<string, string> dayContents)
    {
        var s = new List<IgeNaptarView>();

        try
        {
            if (dayContents.Count > 0)
            {
                // Show only today and 3 future days (4 items total)
                for (int i = 0; i <= 3; i++)
                {
                    var currentDate = date.AddDays(i);
                    var key = currentDate.ToString("MM-dd");

                    // Skip year boundary crossings (December to January)
                    if (date.ToString("MM-dd").StartsWith("12-") && key.StartsWith("01-"))
                    {
                        continue;
                    }

                    if (dayContents.ContainsKey(key))
                    {
                        s.Add(new() { Html = HtmlHelper.WrapHtmlWithFontScale(dayContents[key]) });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** IgeNaptarViewModel EXCEPTION! {ex}");
        }

        return s;
    }
}
