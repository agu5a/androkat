using androkat.hu.Pages;
using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class ImaContentViewModel : BaseView
{
    public ImadsagDto ContentDto { get; set; }

    public ImaContentViewModel(ImadsagDto contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
        //IsSubscribed = isSubscribed;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={ContentDto.Nid}");
}
