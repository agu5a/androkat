using androkat.maui.library.Models.Entities;
using androkat.maui.library.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class FavoriteContentViewModel : BaseView
{
    public FavoriteContentEntity ContentDto { get; set; }

    public FavoriteContentViewModel(FavoriteContentEntity contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"ShowDetailPage?Id={ContentDto.Nid}");
}
