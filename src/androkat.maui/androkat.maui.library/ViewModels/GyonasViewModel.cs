using androkat.maui.library.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class GyonasViewModel : ViewModelBase
{
    private readonly IPageService _pageService;

    public GyonasViewModel(IPageService pageService)
    {
        Items = [];
        _pageService = pageService;
    }

    [ObservableProperty]
    ObservableRangeCollection<GyonasTile> items;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        var s = new List<GyonasTile>
        {
            new("ELMÉLKEDÉS", "icon",_pageService),
            new("IMA", "icon", _pageService),
            new("LELKI TÜKÖR", "icon", _pageService),
            new("JEGYZET", "icon", _pageService),
            new("GYÓNÁS", "icon", _pageService),
            new("TÖRLÉS", "icon", _pageService)
        };

        Items.ReplaceRange(s);
    }
}
