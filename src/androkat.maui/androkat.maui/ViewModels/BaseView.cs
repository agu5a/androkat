using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;

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
