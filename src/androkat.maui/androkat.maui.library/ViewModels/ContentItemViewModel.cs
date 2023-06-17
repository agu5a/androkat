using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class ContentItemViewModel : BaseView
{
    public ContentEntity ContentEntity { get; set; }

    public ContentItemViewModel(ContentEntity contentEntity, bool isSubscribed)
    {
        ContentEntity = contentEntity;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}");
}