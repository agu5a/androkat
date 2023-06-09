using androkat.hu.ViewModels;
using androkat.maui.library.Helpers;
using androkat.maui.library.Services;

namespace androkat.hu.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly PageService _pageService;

    public SettingsPage(SettingsViewModel vm, PageService pageService)
    {
        InitializeComponent();
        BindingContext = vm;
        _pageService = pageService;
    }

    private void maxOffline_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue) && int.TryParse(e.NewTextValue, out int max))
        {
            if (max > 15)
                Preferences.Default.Set("maxOffline", max.ToString());
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var result = await _pageService.DownloadAll();
        if (result != -1)
            await Application.Current.MainPage.DisplayAlert("Siker!", "Minden letöltés kész", "OK");
        else if (result == -1) //nem érhető el az androkat.hu
            await Application.Current.MainPage.DisplayAlert("Hiba!", "Nem érhető el az Androkat szervere! Próbálja meg később, vagy írjon az uzenet@androkat.hu email címre!", "OK");
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Share text file",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath("androkat.db3"))
        });
    }
}