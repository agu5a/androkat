using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class FavoriteListViewModel : ViewModelBase
{
    private readonly IPageService _pageService;
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    int favoriteCount;

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<List<FavoriteContentViewModel>> contents;

    public FavoriteListViewModel(IPageService pageService, ISourceData sourceData)
    {
        _pageService = pageService;
        Contents = [];
        _sourceData = sourceData;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    public async Task<List<FavoriteContentEntity>> GetFavContentsAsync()
    {
        return await _pageService.GetFavoriteContentsAsync();
    }

    private async Task FetchAsync()
    {
        FavoriteCount = await _pageService.GetFavoriteCountAsync();
        var favContents = await _pageService.GetFavoriteContentsAsync();

        if (favContents == null)
        {
            await Shell.Current.DisplayAlert(
                "Hiba",
                "Nincs adat",
                "Bezárás");

            return;
        }

        try
        {
            var temp = ConvertToViewModels(favContents);
            Contents.ReplaceRange([[.. temp]]);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(
                "Hiba",
                "Hiba történt",
                "Bezárás");
        }
    }

    private List<FavoriteContentViewModel> ConvertToViewModels(IEnumerable<FavoriteContentEntity> items)
    {
        var viewmodels = new List<FavoriteContentViewModel>();
        foreach (var item in items)
        {
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            var origImg = item.Image;
            item.Image = idezetSource.Img;
            var viewModel = new FavoriteContentViewModel(item)
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
    public Task Subscribe(FavoriteContentViewModel viewModel) => Task.Run(() => { });
}