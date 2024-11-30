using androkat.maui.library.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;

namespace androkat.maui.library.ViewModels;

public partial class GyonasPrayViewModel : ViewModelBase
{
    private readonly IResourceData _resourceData;

    public GyonasPrayViewModel(IResourceData resourceData)
    {
        _resourceData = resourceData;
    }

    [ObservableProperty]
    string ima;

    public async Task InitializeAsync()
    {
        await FetchAsync();
    }

    async Task FetchAsync()
    {
        Ima = await _resourceData.GetResourceAsString("gyonasima.html");
    }
}