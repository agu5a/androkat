using androkat.maui.library.Models;
using androkat.maui.library.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.hu.ViewModels;

public partial class VideoListViewModel : ViewModelBase
{
    private readonly PageService _pageService;
    private IEnumerable<VideoItemViewModel> _contents;
    private readonly IBrowser _browser;

    [ObservableProperty]
    public string pageTitle;

    [ObservableProperty]
    ObservableRangeCollection<List<VideoItemViewModel>> contents;

    public VideoListViewModel(PageService pageService, IBrowser browser)
    {
        _pageService = pageService;
        Contents = new ObservableRangeCollection<List<VideoItemViewModel>>();
        _browser = browser;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var contents = new List<VideoDto>
        {
            new VideoDto
            {
                Image = "https://img.youtube.com/vi/zxRr7YN02eY/hqdefault.jpg",
                Nid = Guid.NewGuid(),
                Datum = DateTime.Now,
                ChannelId = "UCQzdMyuz0Lf4zo4uGcEujFw",
                Link = "https://www.youtube.com/watch?v=zxRr7YN02eY",
                Forras = "Androkat",
                Cim = "Cím"
            }
        };//await _pageService.GetContentsAsync(Id);

        if (contents == null)
        {
            await Shell.Current.DisplayAlert(
                "Hiba",
                "Nincs adat",
                "Bezárás");

            return;
        }

        _contents = ConvertToViewModels(contents);
        var s = new List<List<VideoItemViewModel>> { _contents.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<VideoItemViewModel> ConvertToViewModels(IEnumerable<VideoDto> items)
    {
        var viewmodels = new List<VideoItemViewModel>();
        foreach (var item in items)
        {
            var viewModel = new VideoItemViewModel(item, _browser)
            {
                VideoDto = item
            };
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }

    [RelayCommand]
    async Task Subscribe(VideoItemViewModel viewModel) => Task.Run(() => { });
}