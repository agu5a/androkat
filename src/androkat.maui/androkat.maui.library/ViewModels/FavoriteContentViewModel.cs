using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class FavoriteContentViewModel(FavoriteContentEntity contentEntity) : BaseView
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public FavoriteContentEntity ContentEntity { get; set; } = contentEntity;

    [RelayCommand]
    Task NavigateToDetail()
    {
        return ContentEntity.Tipus switch
        {
            "6" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}&FromFavorites=true"),
            "15" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}&FromFavorites=true"),
            "23" => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}&FromFavorites=true&IsIma=true"),
            "28" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}&FromFavorites=true"),
            "38" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}&FromFavorites=true"),
            "39" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}&FromFavorites=true"),
            "60" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}&FromFavorites=true"),
            "46" => Shell.Current.GoToAsync($"DetailBook?Id={ContentEntity.Nid}&FromFavorites=true"),
            _ => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}&FromFavorites=true")
        };
    }
}
