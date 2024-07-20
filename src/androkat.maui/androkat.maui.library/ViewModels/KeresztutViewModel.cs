using androkat.maui.library.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace androkat.maui.library.ViewModels;

public partial class KeresztutViewModel : ViewModelBase
{
    private readonly IResourceData _resourceData;

    public KeresztutViewModel(IResourceData resourceData)
    {
        Contents = [];
        _resourceData = resourceData;
    }

    [ObservableProperty]
    ObservableRangeCollection<KeresztutView> contents;

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(2000);

        try
        {
            var htmlContents = new List<KeresztutView>
            {
                new() { Html = await _resourceData.GetResourceAsString("bevezeto.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas1.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas2.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas3.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas4.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas5.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas6.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas7.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas8.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas9.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas10.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas11.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas12.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas13.html") },
                new() { Html = await _resourceData.GetResourceAsString("allomas14.html") }
            };
            Contents.ReplaceRange(htmlContents);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** KeresztutViewModel EXCEPTION! {ex}");
        }
    }
}
