using androkat.maui.library.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;
using System.Text.Json;
using System.Linq;

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

    public async Task InitializeAsync(DateTime day)
    {
        //Delay on first load until window loads
        await Task.Delay(1000);

        var igenaptar = await _resourceData.GetResourceAsString("igenaptar2.json");
        var dayContents = JsonSerializer.Deserialize<Dictionary<string, string>>(igenaptar);

        var s = StartCheck(day, dayContents);
        Contents.ReplaceRange(s);
        if (s.Count > 0)
        {
            var item = s.IndexOf(s.FirstOrDefault(f => f.Html.Contains(day.ToString("MM-dd")), s[0]));
            Position = item;
        }
    }

    private List<IgeNaptarView> StartCheck(DateTime date, Dictionary<string, string> dayContents)
    {
        string tegnapelott2 = date.AddDays(-4).ToString("MM-dd");
        string tegnapelott1 = date.AddDays(-3).ToString("MM-dd");
        string tegnapelott = date.AddDays(-2).ToString("MM-dd");
        string tegnap = date.AddDays(-1).ToString("MM-dd");
        string ma = date.ToString("MM-dd");
        string holnap = date.AddDays(1).ToString("MM-dd");
        string holnaputan = date.AddDays(2).ToString("MM-dd");
        string holnaputan1 = date.AddDays(3).ToString("MM-dd");
        string holnaputan2 = date.AddDays(4).ToString("MM-dd");

        var s = new List<IgeNaptarView>();

        try
        {
            if (dayContents.Count > 0)
            {
                foreach (var keyValue in dayContents)
                {
                    string key = keyValue.Key;
                    if (
                            key.Equals(tegnapelott2)
                                    || key.Equals(tegnapelott1)
                                    || key.Equals(tegnapelott)
                                    || key.Equals(tegnap)
                                    || key.Equals(ma)
                    )
                    {
                        s.Add(new() { Html = keyValue.Value });
                    }
                    if (key.Equals(holnap))
                    {
                        if (ma.StartsWith("12-") && key.StartsWith("01-"))
                        {
                            continue;
                        }
                        s.Add(new() { Html = keyValue.Value });
                    }
                    if (key.Equals(holnaputan))
                    {
                        if (ma.StartsWith("12-") && key.StartsWith("01-"))
                        {
                            continue;
                        }
                        s.Add(new() { Html = keyValue.Value });
                        if (!dayContents.ContainsKey(holnaputan1) && !dayContents.ContainsKey(holnaputan2))
                        {
                            break;
                        }
                    }
                    if (key.Equals(holnaputan1))
                    {
                        if (ma.StartsWith("12-") && key.StartsWith("01-"))
                        {
                            continue;
                        }
                        s.Add(new() { Html = keyValue.Value });
                        if (!dayContents.ContainsKey(holnaputan2))
                        {
                            break;
                        }
                    }
                    if (key.Equals(holnaputan2))
                    {
                        if (ma.StartsWith("12-") && key.StartsWith("01-"))
                        {
                            continue;
                        }
                        s.Add(new() { Html = keyValue.Value });
                        break;
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
