using androkat.hu.Models;
using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class DiscoverViewModel : ViewModelBase
{
    private readonly PageService _pageService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    string text;

    [ObservableProperty]
    ObservableRangeCollection<List<NavigationViewModel>> pages;

    public DiscoverViewModel(PageService pageService, SubscriptionsService subs, CategoriesViewModel categories, ISourceData sourceData)
    {
        _pageService = pageService;
        subscriptionsService = subs;
        Pages = new ObservableRangeCollection<List<NavigationViewModel>>();
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
            new NavigationViewModel(false) { contentImg = "saint", id = "2", detailscim = "Mai Szent" },
            new NavigationViewModel(false) { contentImg = "book", id = "3", detailscim = "Szentek idézetei" }
        };

        Pages.ReplaceRange(new List<List<NavigationViewModel>> { podcastsModels });
    }    
}