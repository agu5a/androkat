using androkat.hu.Pages;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class ImaContentViewModel : BaseView
{
    public ImadsagEntity ContentDto { get; set; }

    public ImaContentViewModel(ImadsagEntity contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={ContentDto.Nid}");
}
