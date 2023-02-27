using androkat.hu.Models;
using androkat.hu.Pages;
using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ShowDetailViewModel : ViewModelBase
{
    public string Id { get; set; }

    private Guid showId;

    private readonly PageService showsService;
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    ContentViewModel contentView;

    [ObservableProperty]
    string textToSearch;

    public ShowDetailViewModel(PageService shows, SubscriptionsService subs, ISourceData sourceData)
    {
        showsService = shows;
        subscriptionsService = subs;
        _sourceData = sourceData;
    }

    internal async Task InitializeAsync()
    {
        if (Id != null)
        {
            showId = new Guid(Id);
        }

        await FetchAsync();
    }

    async Task FetchAsync()
    {
        var item = await showsService.GetContentDtoByIdAsync(showId);

        if (item == null)
        {
            await Shell.Current.DisplayAlert(
                      "Title",
                      "Message",
                      "Close");
            return;
        }

        SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
        var origImg = item.Image;
        item.Image = idezetSource.Img;
        var showViewModel = new ContentViewModel(item, true);
        showViewModel.datum = $"<b>Dátum</b>: {item.Datum.ToString("yyyy-MM-dd")}";
        showViewModel.detailscim = idezetSource.Title;
        showViewModel.contentImg = origImg;
        showViewModel.isFav = false;
        showViewModel.forras = $"<b>Forrás</b>: {idezetSource.Forrasszoveg}";
        showViewModel.type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus));
        ContentView = showViewModel; 
    }

    [RelayCommand]
    async Task AddFavorite()
    {
        
    }

    [RelayCommand]
    async Task Subscribe()
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Title = "AndroKat: " + ContentView.ContentDto.Cim,
            Text = ContentView.ContentDto.Idezet
        });
    }
}
