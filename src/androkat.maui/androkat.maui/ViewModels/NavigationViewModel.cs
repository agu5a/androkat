using androkat.hu.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class NavigationViewModel : ObservableObject
{
    [ObservableProperty]
    bool isSubscribed;

    public string id { get; set; }

    public string contentImg { get; set; }

    public bool isFav { get; set; }

    public string detailscim { get; set; }

    public NavigationViewModel(bool isSubscribed)
    {        
        //IsSubscribed = isSubscribed;
    }

    [RelayCommand]
    Task NavigateToDetail()
    {
        return id switch
        {
            "15" => Shell.Current.GoToAsync($"{nameof(WebPage)}"),
            _ => Shell.Current.GoToAsync($"{nameof(ContentListPage)}?Id={id}"),
        };
    }
}
