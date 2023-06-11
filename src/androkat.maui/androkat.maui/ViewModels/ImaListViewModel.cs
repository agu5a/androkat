using androkat.maui.library.Models;
using androkat.maui.library.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class ImaListViewModel : ViewModelBase
{
    private readonly PageService _pageService;
    private IEnumerable<ImaContentViewModel> _contents;

    [ObservableProperty]
    public string pageTitle;

    [ObservableProperty]
    ObservableRangeCollection<List<ImaContentViewModel>> contents;

    public ImaListViewModel(PageService pageService)
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
        var contents = await _pageService.GetImaContents();

        if (contents == null)
        {
            await Shell.Current.DisplayAlert(
               "Hiba",
                "Nincs adat",
                "Bezárás");

            return;
        }

        _contents = ConvertToViewModels(contents);
        var s = new List<List<ImaContentViewModel>> { _contents.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<ImaContentViewModel> ConvertToViewModels(IEnumerable<ImadsagDto> items)
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
    async Task Subscribe(ContentItemViewModel viewModel) => Task.Run(() => { });
}