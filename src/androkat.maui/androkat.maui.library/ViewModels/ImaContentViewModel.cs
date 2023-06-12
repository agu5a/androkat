using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class ImaContentViewModel : BaseView
{
    public ImadsagEntity ContentDto { get; set; }

    public ImaContentViewModel(ImadsagEntity contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"ShowDetailPage?Id={ContentDto.Nid}");
}
