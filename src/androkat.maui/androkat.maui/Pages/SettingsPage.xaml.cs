using androkat.hu.Helpers;
using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace androkat.hu.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly IPageService _pageService;

    public SettingsPage(SettingsViewModel viewModel, IPageService pageService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _pageService = pageService;
    }

    private void maxOffline_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue) && int.TryParse(e.NewTextValue, out int max) && max > 15)
        {
            Preferences.Default.Set("maxOffline", max.ToString());
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await DownloadAll();
    }

    private async Task DownloadAll()
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
            Title = "Adatbázis fájl megosztása",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath("androkat.db3"))
        });
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        TheTheme.SetTheme();
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        var isDelete = await Shell.Current.DisplayAlert("Törlés", "Biztos törlöd?\n\nInternet kapcsolat kell, hogy az adatbázis újból szinkronizálódjon.", "Igen", "Nem");
        if (isDelete)
        {
            await _pageService.DeleteAllContentAndIma();
            await DownloadAll();
            await SetDbButton();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Offline adatbázis sikeresen törölve.", ToastDuration.Short, 14d);            
            await toast.Show(cancellationTokenSource.Token);
        }
    }

    private async Task SetDbButton()
    {
        try
        {
            int c = await _pageService.GetContentsCount();
            if (c > 0)
            {
                DbDeleteButton.IsEnabled = true;
                DbDeleteButton.Text = string.Format("Törlés ({0})", c);
            }
            else
            {
                DbDeleteButton.Text = "Törlés (0)";
                DbDeleteButton.IsEnabled = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** SettingsPage EXCEPTION! {ex}");
        }
    }
}