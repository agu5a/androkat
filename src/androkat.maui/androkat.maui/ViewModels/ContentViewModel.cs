using androkat.hu.Models;
using androkat.hu.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class BaseView : ObservableObject
{
    [ObservableProperty]
    bool isSubscribed;

    public string contentImg { get; set; }

    public bool isFav { get; set; }

    public Activities type { get; set; }

    public string detailscim { get; set; }

    public string datum { get; set; }

    public string forras { get; set; }        
}

public partial class FavoriteContentViewModel : BaseView
{
    public FavoriteContentDto ContentDto { get; set; }

    public FavoriteContentViewModel(FavoriteContentDto contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
        //IsSubscribed = isSubscribed;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={ContentDto.Nid}");
}

public partial class ContentViewModel : BaseView
{
    public ContentDto ContentDto { get; set; }

    public ContentViewModel(ContentDto contentDto, bool isSubscribed)
    {
        ContentDto = contentDto;
        //IsSubscribed = isSubscribed;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={ContentDto.Nid}");
}