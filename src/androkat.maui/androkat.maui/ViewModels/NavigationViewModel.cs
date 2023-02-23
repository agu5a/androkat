using androkat.hu.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class NavigationViewModel : ObservableObject
{
    public string id { get; set; }

    public string contentImg { get; set; }

    public bool isFav { get; set; }

    public string detailscim { get; set; }

    public NavigationViewModel()
    {        
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ContentListPage)}?Id={id}");
}
