using androkat.hu.Models;
using androkat.hu.Pages;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class FavoriteContentViewModel : BaseView
{
    public FavoriteContentDto ContentDto { get; set; }

    public FavoriteContentViewModel(FavoriteContentDto contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
        //IsSubscribed = isSubscribed;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={ContentDto.Nid}");
}
