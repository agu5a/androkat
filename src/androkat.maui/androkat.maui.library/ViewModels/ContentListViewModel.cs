using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ContentListViewModel : ViewModelBase
{
    private readonly IPageService _pageService;
    private readonly ISourceData _sourceData;
    private readonly IAndrokatService _androkatService;

    public string Id { get; set; }

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<List<ContentItemViewModel>> contents;

    public ContentListViewModel(IPageService pageService, ISourceData sourceData, IAndrokatService androkatService)
    {
        _pageService = pageService;
        Contents = [];
        _sourceData = sourceData;
        _androkatService = androkatService;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await CheckNewVersion();
        await FetchAsync();
    }

    private async Task CheckNewVersion()
    {
        try
        {
            if (Settings.LastUpdate < DateTime.Now.AddHours(-1))
            {
                var serverInfo = await _androkatService.GetServerInfo();
                var ver = serverInfo.Find(f => f.Key == "versionmaui");
                var newVersion = int.Parse(ver.Value);

                int curVersion = _pageService.GetVersion();
                if (curVersion < newVersion)
                {
                    var result = await Shell.Current.DisplayAlert("Frissítés", "Új verzió érhető el. Szeretné frissíteni?", "Igen", "Nem");
                    if (result)
                    {
                        await Browser.OpenAsync(ConsValues.AndrokatMarket);
                    }
                }
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Hiba", "??", "Bezárás");
        }
    }

    private async Task FetchAsync()
    {
        var contentsTemp = await _pageService.GetContentsAsync(Id);

        if (contentsTemp == null)
        {
            await Shell.Current.DisplayAlert("Hiba", "Nincs adat", "Bezárás");
            return;
        }

        var temp = ConvertToViewModels(contentsTemp);
        var s = new List<List<ContentItemViewModel>> { temp.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<ContentItemViewModel> ConvertToViewModels(IEnumerable<ContentEntity> items)
    {
        var viewmodels = new List<ContentItemViewModel>();
        foreach (var item in items)
        {
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            var origImg = item.Image;
            item.Image = idezetSource.Img;
            var viewModel = new ContentItemViewModel(item)
            {
                datum = $"Dátum: {item.Datum:yyyy-MM-dd}",
                detailscim = idezetSource.Title,
                contentImg = origImg,
                isFav = false,
                forras = $"Forrás: {idezetSource.Forrasszoveg}",
                type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus))
            };
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    [RelayCommand]
    public Task Subscribe(ContentItemViewModel viewModel) => Task.Run(() => { });
}