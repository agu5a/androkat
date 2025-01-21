#nullable enable
using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections;

namespace androkat.maui.library.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ContentListViewModel : ViewModelBase
{
    private readonly IPageService _pageService;
    private readonly ISourceData _sourceData;
    private readonly IAndrokatService _androkatService;

    public ContentListViewModel(IDispatcher dispatcher, IPageService pageService, ISourceData sourceData, IAndrokatService androkatService)
    {
        Dispatcher = dispatcher;
        _pageService = pageService;
        Contents = [];
        _sourceData = sourceData;
        _androkatService = androkatService;

        BindingBase.EnableCollectionSynchronization(Contents, null, ObservableCollectionCallback);
    }

    protected IDispatcher Dispatcher { get; }
    public string? Id { get; set; }

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string? pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<List<ContentItemViewModel>> contents;

    public async Task InitializeAsync(bool returnVisited)
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await UpdateData();
        await CheckNewVersion();
        await FetchAsync(returnVisited);
    }

    private async Task UpdateData()
    {
        try
        {
            if (Settings.LastUpdate < DateTime.Now.AddMinutes(-5))
            {
                var result = await _pageService.DownloadAll();
                if (result == -1)
                {
                    await Shell.Current.DisplayAlert("Hiba!", "Nem érhető el az Androkat szervere! Próbálja meg később, vagy írjon az uzenet@androkat.hu email címre!", "OK");
                }

                Settings.LastUpdate = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hiba", ex.Message, "Bezárás");
        }
    }

    private async Task CheckNewVersion()
    {
        try
        {
            if (Settings.LastVersionCheck < DateTime.Now.AddHours(-1))
            {
                var serverInfo = await _androkatService.GetServerInfo();
                var ver = serverInfo.Find(f => f.Key == "versionmaui");
                if (ver is null)
                {
                    return;
                }
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
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hiba", ex.Message, "Bezárás");
        }
    }

    public async Task FetchAsync(bool returnVisited)
    {
        var contentsTemp = await _pageService.GetContentsAsync(Id!, returnVisited);

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

    //Ensure Observable Collection is thread-safe https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>")]
    void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
    {
        Dispatcher.Dispatch(accessMethod);
    }
}