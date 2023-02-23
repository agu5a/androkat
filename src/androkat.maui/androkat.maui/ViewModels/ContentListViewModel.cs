using androkat.hu.Models;
using androkat.hu.Pages;
using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Util;
using MvvmHelpers;
using System.Collections.Generic;
using static Android.Content.ClipData;

namespace androkat.hu.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ContentListViewModel : ViewModelBase
{
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;
    private IEnumerable<ContentViewModel> _contents;
    private readonly ISourceData _sourceData;

    public string Id { get; set; }

    [ObservableProperty]
    ObservableRangeCollection<List<ContentViewModel>> contents;

    public ContentListViewModel(ShowsService shows, SubscriptionsService subs, CategoriesViewModel categories, ISourceData sourceData)
    {
        showsService = shows;
        subscriptionsService = subs;
        Contents = new ObservableRangeCollection<List<ContentViewModel>>();
        _sourceData = sourceData;
    }

    internal async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var podcastsModels = await showsService.GetShowsAsync(Id);

        if (podcastsModels == null)
        {
            await Shell.Current.DisplayAlert(
                "Error_Title",
                "Error_Message",
                "Close");

            return;
        }

        _contents = ConvertToViewModels(podcastsModels);
        var s = new List<List<ContentViewModel>> { _contents.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<ContentViewModel> ConvertToViewModels(IEnumerable<NapiElmelkedesDto> items)
    {
        var viewmodels = new List<ContentViewModel>();
        foreach (var item in items)
        {
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.tipus));
            var origImg = item.img;
            item.img = idezetSource.Img;                       
            var showViewModel = new ContentViewModel(item, true); 
            showViewModel.datum = $"Dátum: {item.datum.ToString("yyyy-MM-dd")}"; 
            showViewModel.detailscim = idezetSource.Title;
            showViewModel.contentImg = origImg;
            showViewModel.isFav = false;
            showViewModel.forras = $"Forrás: {idezetSource.Forrasszoveg}";
            showViewModel.type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.tipus));
            viewmodels.Add(showViewModel);
        }

        return viewmodels;
    }
}