using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class ContentItemViewModel : BaseView
{
    public ContentEntity ContentDto { get; set; }

    public ContentItemViewModel(ContentEntity contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"ShowDetailPage?Id={ContentDto.Nid}");
}