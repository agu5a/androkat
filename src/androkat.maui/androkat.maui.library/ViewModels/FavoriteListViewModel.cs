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
        Contents = new ObservableRangeCollection<List<FavoriteContentViewModel>>();
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

        var temp = ConvertToViewModels(favContents);
        Contents.ReplaceRange(new List<List<FavoriteContentViewModel>> { temp.ToList() });
    }

    private List<FavoriteContentViewModel> ConvertToViewModels(IEnumerable<FavoriteContentEntity> items)
    {
        var viewmodels = new List<FavoriteContentViewModel>();
        foreach (var item in items)
        {
            SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            var origImg = item.Image;
            item.Image = idezetSource.Img;
            var viewModel = new FavoriteContentViewModel(item, true);
            viewModel.datum = $"Dátum: {item.Datum.ToString("yyyy-MM-dd")}";
            viewModel.detailscim = idezetSource.Title;
            viewModel.contentImg = origImg;
            viewModel.isFav = false;
            viewModel.forras = $"Forrás: {idezetSource.Forrasszoveg}";
            viewModel.type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus));
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }

    [RelayCommand]
    public Task Subscribe(FavoriteContentViewModel viewModel) => Task.Run(() => { });
}