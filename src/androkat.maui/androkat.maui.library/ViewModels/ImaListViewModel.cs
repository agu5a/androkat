using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    ObservableRangeCollection<List<ImaContentViewModel>> contents;

    public ImaListViewModel(IPageService pageService)
    {
        _pageService = pageService;
        Contents = new ObservableRangeCollection<List<ImaContentViewModel>>();
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var imaContents = await _pageService.GetImaContents();

        if (imaContents == null)
        {
            await Shell.Current.DisplayAlert(
               "Hiba",
                "Nincs adat",
                "Bezárás");

            return;
        }

        var temp = ConvertToViewModels(imaContents);
        var s = new List<List<ImaContentViewModel>> { temp.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<ImaContentViewModel> ConvertToViewModels(IEnumerable<ImadsagEntity> items)
    {
        var viewmodels = new List<ImaContentViewModel>();
        foreach (var item in items)
        {
            var viewModel = new ImaContentViewModel(item, true);
            viewModel.datum = $"Dátum: {item.Datum.ToString("yyyy-MM-dd")}";
            viewModel.detailscim = "Imádságok";
            viewModel.isFav = false;
            viewModel.type = Activities.ima;
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }

    [RelayCommand]
    public Task Subscribe(ImaContentViewModel viewModel) => Task.Run(() => { });
}