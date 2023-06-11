using androkat.hu.Pages;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class FavoriteContentViewModel : BaseView
{
    public FavoriteContentEntity ContentDto { get; set; }

    public FavoriteContentViewModel(FavoriteContentEntity contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={ContentDto.Nid}");
}
