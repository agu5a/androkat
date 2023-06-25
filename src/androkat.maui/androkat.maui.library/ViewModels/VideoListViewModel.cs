using androkat.maui.library.Abstraction;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class VideoListViewModel : ViewModelBase
{
    private IEnumerable<VideoItemViewModel> _contents;
    private readonly IBrowser _browser;

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<List<VideoItemViewModel>> contents;

    public VideoListViewModel(IBrowser browser)
    {
        Contents = new ObservableRangeCollection<List<VideoItemViewModel>>();
        _browser = browser;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        FetchAsync();
    }

    private void FetchAsync()
    {
        var entities = new List<VideoEntity>
        {
            new VideoEntity
            {
                Image = "https://img.youtube.com/vi/zxRr7YN02eY/hqdefault.jpg",
                Nid = Guid.NewGuid(),
                Datum = DateTime.Now,
                ChannelId = "UCQzdMyuz0Lf4zo4uGcEujFw",
                Link = "https://www.youtube.com/watch?v=zxRr7YN02eY",
                Forras = "Androkat",
                Cim = "Cím"
            }
        };

        _contents = ConvertToViewModels(entities);
        var s = new List<List<VideoItemViewModel>> { _contents.ToList() };
        Contents.ReplaceRange(s);
    }

    private List<VideoItemViewModel> ConvertToViewModels(IEnumerable<VideoEntity> items)
    {
        var viewmodels = new List<VideoItemViewModel>();
        foreach (var item in items)
        {
            var viewModel = new VideoItemViewModel(item, _browser)
            {
                VideoEntity = item
            };
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }

    [RelayCommand]
    public Task Subscribe(VideoItemViewModel viewModel) => Task.Run(() => { });
}