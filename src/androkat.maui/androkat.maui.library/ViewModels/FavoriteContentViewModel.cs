using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class FavoriteContentViewModel(FavoriteContentEntity contentEntity) : BaseView
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public FavoriteContentEntity ContentEntity { get; set; } = contentEntity;

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}");
}
