using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class VideoItemViewModel(VideoEntity contentEntity) : BaseView
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public VideoEntity VideoEntity { get; set; } = contentEntity;

    public string FormattedDate => VideoEntity.Datum.ToString("yyyy.MM.dd");

    [RelayCommand]
    async Task OpenVideoInBrowser()
    {
        await Shell.Current.GoToAsync($"WebViewPage?Url={Uri.EscapeDataString(VideoEntity.Link)}");
    }
}