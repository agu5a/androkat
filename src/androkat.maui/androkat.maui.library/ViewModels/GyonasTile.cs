using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class GyonasTile(string name, string icon) : ObservableObject
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
            "GYÓNÁS" => Shell.Current.DisplayAlert("Hiba", "GYÓNÁS Nincs még kész", "Bezárás"),
            "ELMÉLKEDÉS" => Shell.Current.GoToAsync($"GyonasMeditationPage"),
            "IMA" => Shell.Current.DisplayAlert("Hiba", "IMA Nincs még kész", "Bezárás"),
            "JEGYZET" => Shell.Current.DisplayAlert("Hiba", "JEGYZET Nincs még kész", "Bezárás"),
            //TÖRLÉS
            _ => Shell.Current.DisplayAlert("Hiba", "TÖRLÉS Nincs még kész", "Bezárás"),
        };
    }
}