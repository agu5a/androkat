using androkat.maui.library.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class GyonasNotesViewModel : ViewModelBase
{
    private readonly IPageService _pageService;

    public GyonasNotesViewModel(IPageService pageService)
    {
        _pageService = pageService;
    }

    public async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    public bool IsSaved { get; set; }

    [ObservableProperty]
    string notes = string.Empty;

    [ObservableProperty]
    string jegyzetCharCount = string.Empty;

    [RelayCommand]
    async Task Save()
    {
        await _pageService.UpsertGyonasiJegyzet(Notes);
        IsSaved = true;
        await Shell.Current.DisplayAlertAsync("Mentés", "Mentés sikerült", "OK");
    }

    [RelayCommand]
    async Task Cancel()
    {
        await _pageService.DeleteUserGyonas(true, false);
        Notes = "";
        JegyzetCharCount = "0/1000";
        IsSaved = true;
        await Shell.Current.DisplayAlertAsync("Törlés", "Törlés sikerült", "OK");
    }

    public void JegyzetTextChanged()
    {
        if (Notes.Length > 0)
        {
            Notes = Notes.Replace("|", "\n");
            JegyzetCharCount = $"{Notes.Length}/1000";
        }
        else
        {
            Notes = "";
            JegyzetCharCount = "0/1000";
        }
    }

    private async Task FetchAsync()
    {
        var result = await _pageService.GetGyonasiJegyzet();
        if (result != null && result.Jegyzet.Length > 0)
        {
            Notes = result.Jegyzet.Replace("|", "\n");
            JegyzetCharCount = $"{result.Jegyzet.Length}/1000";
        }
        else
        {
            Notes = "";
            JegyzetCharCount = "0/1000";
        }
    }
}