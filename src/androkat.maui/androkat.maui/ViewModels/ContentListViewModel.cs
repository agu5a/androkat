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
    private IEnumerable<ContentItemViewModel> _contents;
    private readonly ISourceData _sourceData;

    public string Id { get; set; }

    [ObservableProperty]
    string text;

    [ObservableProperty]
    public string pageTitle;

    [ObservableProperty]
    ObservableRangeCollection<List<ContentItemViewModel>> contents;

    public ContentListViewModel(PageService pageService, ISourceData sourceData)
    {
        _pageService = pageService;
        Contents = new ObservableRangeCollection<List<ContentItemViewModel>>();
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
        var s = new List<List<ContentItemViewModel>> { _contents.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<ContentItemViewModel> ConvertToViewModels(IEnumerable<ContentDto> items)
    {
        var viewmodels = new List<ContentItemViewModel>();
        foreach (var item in items)
        {
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            var origImg = item.Image;
            item.Image = idezetSource.Img;
            var showViewModel = new ContentItemViewModel(item, true); 
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