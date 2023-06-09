using androkat.hu.Models;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Responses;
using androkat.maui.library.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CategoryViewModel : ViewModelBase
{

    [ObservableProperty]
    string text;

    public string Id { get; set; }

    [ObservableProperty]
    Category category;

    [ObservableProperty]
    List<ShowViewModel> shows;

    readonly PageService showsService;
    //readonly SubscriptionsService subscriptionsService;

    public CategoryViewModel(PageService shows /*,SubscriptionsService subs*/)
    {
        showsService = shows;
        //subscriptionsService = subs;
    }


    public async Task InitializeAsync()
    {
        await LoadCategoryAsync();
        var shows = await showsService.GetShowsByCategoryAsync(new Guid(Id));

        Shows = LoadShows(shows);
    }

    async Task LoadCategoryAsync()
    {
        //var allCategories = await showsService.GetAllCategories();
        Category = new Category(new CategoryResponse { }) { };// allCategories?.First(c => c.Id == new Guid(Id));
    }

    [RelayCommand]
    async Task Subscribe(ShowViewModel vm)
    {
        //await subscriptionsService.UnSubscribeFromShowAsync(vm.Show);
        OnPropertyChanged(nameof(vm.IsSubscribed));
    }

    [RelayCommand]
    async void Search()
    {
        var shows = await showsService.SearchShowsAsync(new Guid(Id), Text);
        Shows = LoadShows(shows);
    }

    List<ShowViewModel> LoadShows(IEnumerable<Show> shows)
    {
        var showList = new List<ShowViewModel>();
        if (shows == null)
        {
            return showList;
        }

        foreach (var show in shows)
        {
            //var showVM = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id));
            //showList.Add(showVM);
        }

        return showList;
    }
}
