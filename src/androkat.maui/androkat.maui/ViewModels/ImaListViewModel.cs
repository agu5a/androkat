using androkat.maui.library.Abstraction;
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
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    string text;

    [ObservableProperty]
    public string pageTitle;

    [ObservableProperty]
    ObservableRangeCollection<List<ImaContentViewModel>> contents;

    public ImaListViewModel(PageService pageService, ISourceData sourceData)
    {
        _pageService = pageService;
        Contents = new ObservableRangeCollection<List<ImaContentViewModel>>();
        _sourceData = sourceData;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        //if (_contents?.Any() ?? false)
        //    return;

        var contents = await _pageService.GetImaContents();

        if (contents == null)
        {
            await Shell.Current.DisplayAlert(
                "Error_Title",
                "Error_Message",
                "Close");

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
            //SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            //var origImg = item.Image;
            //item.Image = idezetSource.Img;
            var showViewModel = new ImaContentViewModel(item, true); 
            showViewModel.datum = $"Dátum: {item.Datum.ToString("yyyy-MM-dd")}";
            showViewModel.detailscim = "Imádságok"; // idezetSource.Title;
            //showViewModel.contentImg = origImg;
            showViewModel.isFav = false;
            //showViewModel.forras = $"Forrás: {idezetSource.Forrasszoveg}";
            showViewModel.type = Activities.ima;
            viewmodels.Add(showViewModel);
        }

        return viewmodels;
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
}