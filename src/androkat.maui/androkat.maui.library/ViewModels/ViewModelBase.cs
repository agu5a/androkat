using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading;

namespace androkat.maui.library.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected readonly CancellationTokenSource _cancellationTokenSource = new();

    [ObservableProperty]
    string title;

    public void OnDisappearing()
    {
        _cancellationTokenSource.Dispose();
    }
}