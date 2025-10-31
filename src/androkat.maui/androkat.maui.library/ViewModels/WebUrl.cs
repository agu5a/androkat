using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class WebUrl(string name, string url) : ObservableObject
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public string Name { get; } = name;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public string Url { get; } = url;

    [RelayCommand]
    async Task NavigateToWeb()
    {
        await Shell.Current.GoToAsync($"WebViewPage?Url={Uri.EscapeDataString(Url)}");
    }
}