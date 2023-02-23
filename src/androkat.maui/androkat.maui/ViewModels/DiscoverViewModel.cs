using androkat.hu.Models;
using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class DiscoverViewModel : ViewModelBase
{
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    string text;

    [ObservableProperty]
    ObservableRangeCollection<List<NavigationViewModel>> podcastsGroup;

    public DiscoverViewModel(ShowsService shows, SubscriptionsService subs, CategoriesViewModel categories, ISourceData sourceData)
    {
        showsService = shows;
        subscriptionsService = subs;
        PodcastsGroup = new ObservableRangeCollection<List<NavigationViewModel>>();
        _sourceData = sourceData;
    }

    internal async Task InitializeAsync()
    {
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var podcastsModels = new List<NavigationViewModel>
        {
            new NavigationViewModel(false) { contentImg = "book", id = "0", detailscim = "Elmélkedés" },
            new NavigationViewModel(false) { contentImg = "gift", id = "1", detailscim = "AJÁNLATAINK" },
            new NavigationViewModel(false) { contentImg = "saint", id = "2", detailscim = "Mai Szent" }
        };

        PodcastsGroup.ReplaceRange(new List<List<NavigationViewModel>> { podcastsModels });
    }    
}