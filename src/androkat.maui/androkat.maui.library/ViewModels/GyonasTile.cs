using androkat.maui.library.Abstraction;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class GyonasTile(string name, string icon, IPageService pageService) : ObservableObject
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public string Name { get; } = name;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    public string Icon { get; } = icon;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    [RelayCommand]
    Task Navigate(string page)
    {
        return page switch
        {
            "LELKI TÜKÖR" => Shell.Current.DisplayAlert("Hiba", "LELKI TÜKÖR Nincs még kész", "Bezárás"),
            "GYÓNÁS" => Shell.Current.GoToAsync("GyonasFinishPage"),
            "ELMÉLKEDÉS" => Shell.Current.GoToAsync("ContentListPage?Id=1"),
            "IMA" => Shell.Current.GoToAsync("GyonasPrayPage"),
            "JEGYZET" => Shell.Current.DisplayAlert("Hiba", "JEGYZET Nincs még kész", "Bezárás"),
            //TÖRLÉS
            _ => GyonasTorles(),
        };
    }

    private async Task GyonasTorles()
    {
        var isDelete = await Shell.Current.DisplayAlert("Törlés", "Parancsok, jegyzet törlése!", "Törlés", "Vissza");
        if (isDelete)
        {
            await pageService.DeleteUserGyonas(true, true);

            using var cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Törlés sikerült", ToastDuration.Short, 14d);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}