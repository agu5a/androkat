using CommunityToolkit.Mvvm.ComponentModel;

namespace androkat.maui.library.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    string title;

    [ObservableProperty]
    string subtitle;

    [ObservableProperty]
    string icon;

    [ObservableProperty]
    bool canLoadMore;

    [ObservableProperty]
    string header;

    [ObservableProperty]
    string footer;
}