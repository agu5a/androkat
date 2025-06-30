using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class ContentItemViewModel(ContentEntity contentEntity) : BaseView
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public ContentEntity ContentEntity { get; set; } = contentEntity;

    public bool IsRead => ContentEntity.IsRead;
    public FontAttributes TextFontAttributes => IsRead ? FontAttributes.None : FontAttributes.Bold;

    public void UpdateReadStatus()
    {
        OnPropertyChanged(nameof(IsRead));
        OnPropertyChanged(nameof(TextFontAttributes));
    }

    [RelayCommand]
    Task NavigateToDetail()
    {
        return ContentEntity.Tipus switch
        {
            "6" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}"),
            "15" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}"),
            "28" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}"),
            "38" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}"),
            "39" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}"),
            "60" => Shell.Current.GoToAsync($"DetailAudio?Id={ContentEntity.Nid}"),
            "46" => Shell.Current.GoToAsync($"DetailBook?Id={ContentEntity.Nid}"),
            _ => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}")
        };
    }
}