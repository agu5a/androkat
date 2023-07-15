using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class ImaContentViewModel : BaseView
{
    public ImadsagEntity ContentEntity { get; set; }

    public ImaContentViewModel(ImadsagEntity contentEntity)
    {
        ContentEntity = contentEntity;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"DetailPage?Id={ContentEntity.Nid}&IsIma=true");
}
