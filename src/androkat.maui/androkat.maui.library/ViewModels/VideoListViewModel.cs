using androkat.maui.library.Abstraction;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Globalization;

namespace androkat.maui.library.ViewModels;

public partial class VideoListViewModel : ViewModelBase
{
    private readonly IAndrokatService _androkatService;
    private int _offset = 0;
    private bool _isLoadingMore = false;

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<VideoItemViewModel> contents;

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    bool isLoadingMore;

    public VideoListViewModel(IAndrokatService androkatService)
    {
        Contents = [];
        _androkatService = androkatService;
        PageTitle = "Videók";
    }

    public async Task InitializeAsync()
    {
        if (Contents.Any())
            return;

        await LoadVideosAsync();
    }

    [RelayCommand]
    public async Task LoadMoreAsync()
    {
        if (_isLoadingMore)
            return;

        _isLoadingMore = true;
        IsLoadingMore = true;

        try
        {
            await LoadVideosAsync();
        }
        finally
        {
            _isLoadingMore = false;
            IsLoadingMore = false;
        }
    }

    private async Task LoadVideosAsync()
    {
        if (_offset == 0)
            IsLoading = true;

        try
        {
            var videoResponses = await _androkatService.GetVideos(_offset);

            if (videoResponses?.Any() == true)
            {
                var videoEntities = ConvertToEntities(videoResponses);
                var viewModels = ConvertToViewModels(videoEntities);

                Contents.AddRange(viewModels);
                _offset += videoResponses.Count;
            }
        }
        catch (Exception ex)
        {
            // Log error - you might want to add proper logging here
            System.Diagnostics.Debug.WriteLine($"Error loading videos: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    private List<VideoEntity> ConvertToEntities(IEnumerable<VideoResponse> responses)
    {
        var entities = new List<VideoEntity>();

        foreach (var response in responses)
        {
            // Debug: log the raw date string from API
            System.Diagnostics.Debug.WriteLine($"Raw date from API: '{response.Date}' for video: '{response.Cim}'");

            var entity = new VideoEntity
            {
                Nid = Guid.NewGuid(),
                Cim = response.Cim ?? string.Empty,
                Image = response.Img ?? string.Empty,
                Link = response.Videolink ?? string.Empty,
                Forras = response.Forras ?? string.Empty,
                ChannelId = response.ChannelId ?? string.Empty,
                Datum = DateTime.TryParse(response.Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : DateTime.Now
            };

            // Debug: log the parsed date
            System.Diagnostics.Debug.WriteLine($"Parsed date: {entity.Datum:yyyy-MM-dd} for video: '{response.Cim}'");

            entities.Add(entity);
        }

        return entities;
    }

    private List<VideoItemViewModel> ConvertToViewModels(IEnumerable<VideoEntity> items)
    {
        var viewmodels = new List<VideoItemViewModel>();
        foreach (var item in items)
        {
            var viewModel = new VideoItemViewModel(item)
            {
                VideoEntity = item
            };
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }
}