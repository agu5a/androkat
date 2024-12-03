using androkat.maui.library.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;

namespace androkat.maui.library.ViewModels;

public partial class GyonasFinishViewModel : ViewModelBase
{
    private readonly IResourceData _resourceData;

    public GyonasFinishViewModel(IResourceData resourceData)
    {
        _resourceData = resourceData;
    }

    [ObservableProperty]
    string gyonasSzoveg;

    [ObservableProperty]
    bool isChecked = true;

    public async Task InitializeAsync()
    {
        await FetchAsync();
    }

    async Task FetchAsync()
    {
        if (IsChecked)
        {
            GyonasSzoveg = await _resourceData.GetResourceAsString("gyonas.html");
        }
        else
        {
            GyonasSzoveg = await _resourceData.GetResourceAsString("gyonasrovid.html");
        }
    }
}