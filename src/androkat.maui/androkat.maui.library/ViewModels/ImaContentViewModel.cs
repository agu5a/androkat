using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class ImaContentViewModel : BaseView
{
    public ImadsagEntity ContentEntity { get; set; }

    public ImaContentViewModel(ImadsagEntity contentEntity, bool isSubscribed)
    {
        ContentEntity = contentEntity;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"ShowDetailPage?Id={ContentEntity.Nid}");
}
