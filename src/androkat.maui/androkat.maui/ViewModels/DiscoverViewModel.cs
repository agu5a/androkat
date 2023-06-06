using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class DiscoverViewModel : ViewModelBase
{
    private readonly PageService _pageService;
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    string text;

    [ObservableProperty]
    ObservableRangeCollection<List<NavigationViewModel>> pages;

    public DiscoverViewModel(PageService pageService, CategoriesViewModel categories, ISourceData sourceData)
    {
        _pageService = pageService;
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
            new NavigationViewModel(false) { contentImg = "book", id = "3", detailscim = "Szentek idézetei" },
            new NavigationViewModel(false) { contentImg = "news1", id = "4", detailscim = "Katolikus Hírek" },
            new NavigationViewModel(false) { contentImg = "blog", id = "5", detailscim = "Blog, magazin" },
            new NavigationViewModel(false) { contentImg = "ic_smile", id = "6", detailscim = "Humor" },
            new NavigationViewModel(false) { contentImg = "candle", id = "7", detailscim = "Imádság" },
            new NavigationViewModel(false) { contentImg = "ic_volume_up_24dp", id = "8", detailscim = "Hanganyagok" },
            new NavigationViewModel(false) { contentImg = "ic_video_collection_24dp", id = "9", detailscim = "Videók" },
            new NavigationViewModel(false) { contentImg = "ic_radio_24dp", id = "10", detailscim = "Webrádiók" },
            new NavigationViewModel(false) { contentImg = "book", id = "11", detailscim = "Könyvolvasó" },
            new NavigationViewModel(false) { contentImg = "confession", id = "12", detailscim = "Gyónás" },
            new NavigationViewModel(false) { contentImg = "book", id = "13", detailscim = "Igenaptár" },
            new NavigationViewModel(false) { contentImg = "crossroad", id = "14", detailscim = "Keresztút" },
            new NavigationViewModel(false) { contentImg = "internet", id = "15", detailscim = "Weboldalak" }

        };

        Pages.ReplaceRange(new List<List<NavigationViewModel>> { podcastsModels });
    }

    /*[RelayCommand]
    async Task Search()
    {
        IEnumerable<Show> list;
        if (string.IsNullOrWhiteSpace(Text))
        {
            list = await showsService.GetShowsAsync();
        }
        else
        {
            list = await showsService.SearchShowsAsync(Text);
        }

        if (list != null)
        {
            UpdatePodcasts(ConvertToViewModels(list));
        }
    }*/

    [RelayCommand]
    async Task Subscribe(ShowViewModel showViewModel) => Task.Run(() => { });//showViewModel.IsSubscribed = await subscriptionsService.UnSubscribeFromShowAsync(showViewModel.Show);

    //[RelayCommand]
    //Task SeeAllCategories() => Shell.Current.GoToAsync($"{nameof(CategoriesPage)}");
}