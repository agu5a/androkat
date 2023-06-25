using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class FavoriteContentViewModel : BaseView
{
    public FavoriteContentEntity ContentEntity { get; set; }

    public FavoriteContentViewModel(FavoriteContentEntity contentEntity, bool isSubscribed)
    {
        ContentEntity = contentEntity;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}");
}
