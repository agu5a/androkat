using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading;

namespace androkat.maui.library.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected readonly CancellationTokenSource _cancellationTokenSource = new();

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

    public void OnDisappearing()
    {
        _cancellationTokenSource.Dispose();
    }
}