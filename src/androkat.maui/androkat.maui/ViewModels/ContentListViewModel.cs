using androkat.hu.Models;
using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ContentListViewModel : ViewModelBase
{
    private readonly PageService _pageService;
    private readonly SubscriptionsService subscriptionsService;
    private IEnumerable<ContentViewModel> _contents;
    private readonly ISourceData _sourceData;

    public string Id { get; set; }

    [ObservableProperty]
    string text;

    [ObservableProperty]
    public string pageTitle;

    [ObservableProperty]
    ObservableRangeCollection<List<ContentViewModel>> contents;

    public ContentListViewModel(PageService pageService, SubscriptionsService subs, ISourceData sourceData)
    {
        _pageService = pageService;
        subscriptionsService = subs;
        Contents = new ObservableRangeCollection<List<ContentViewModel>>();
        _sourceData = sourceData;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var contents = await _pageService.GetContentsAsync(Id);

        if (contents == null)
        {
            await Shell.Current.DisplayAlert(
                "Error_Title",
                "Error_Message",
                "Close");

            return;
        }

        _contents = ConvertToViewModels(contents);
        var s = new List<List<ContentViewModel>> { _contents.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<ContentViewModel> ConvertToViewModels(IEnumerable<ContentDto> items)
    {
        var viewmodels = new List<ContentViewModel>();
        foreach (var item in items)
        {
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            var origImg = item.Image;
            item.Image = idezetSource.Img;
            var showViewModel = new ContentViewModel(item, true); 
            showViewModel.datum = $"Dátum: {item.Datum.ToString("yyyy-MM-dd")}";
            showViewModel.detailscim = idezetSource.Title;
            showViewModel.contentImg = origImg;
            showViewModel.isFav = false;
            showViewModel.forras = $"Forrás: {idezetSource.Forrasszoveg}";
            showViewModel.type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus));
            viewmodels.Add(showViewModel);
        }

        return viewmodels;
    }
}