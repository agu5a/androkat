using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class ImaListViewModel : ViewModelBase
{
    private readonly IPageService _pageService;

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<ImaContentViewModel> contents;

    public ImaListViewModel(IPageService pageService)
    {
        _pageService = pageService;
        Contents = [];
    }

    public async Task InitializeAsync(int pageNumber, int pageSize)
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync(pageNumber, pageSize);
    }

    public async Task FetchAsync(int pageNumber, int pageSize)
    {
        var imaContents = await _pageService.GetImaContents(pageNumber, pageSize);
        if (imaContents.Count == 0)
        {
            await Shell.Current.DisplayAlert(
               "Program hiba",
                "Imák lekérdezése sikertelen, kérjük próbálja újra később.",
                "Bezárás");

            return;
        }

        var temp = ConvertToViewModels(imaContents);
        Contents.AddRange(temp);
    }

    private static List<ImaContentViewModel> ConvertToViewModels(IEnumerable<ImadsagEntity> items)
    {
        var viewmodels = new List<ImaContentViewModel>();
        foreach (var item in items)
        {
            var viewModel = new ImaContentViewModel(item)
            {
                datum = $"Dátum: {item.Datum:yyyy-MM-dd}",
                detailscim = "Imádságok",
                isFav = false,
                type = Activities.ima,
                forras = string.Empty // IMA items don't have a source
            };
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }
}