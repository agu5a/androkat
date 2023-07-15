using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class GyonasTile : ObservableObject
{
    public GyonasTile(string name, string icon)
    {
        Name = name;
        Icon = icon;
    }

    public string Name { get; }
    public string Icon { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    [RelayCommand]
    Task Navigate(string page)
    {
        return page switch
        {
            "LELKI TÜKÖR" => Shell.Current.DisplayAlert("Hiba", "LELKI TÜKÖR Nincs még kész", "Bezárás"),
            "GYÓNÁS" => Shell.Current.DisplayAlert("Hiba", "GYÓNÁS Nincs még kész", "Bezárás"),
            "ELMÉLKEDÉS" => Shell.Current.DisplayAlert("Hiba", "ELMÉLKEDÉS Nincs még kész", "Bezárás"),
            "IMA" => Shell.Current.DisplayAlert("Hiba", "IMA Nincs még kész", "Bezárás"),
            "JEGYZET" => Shell.Current.DisplayAlert("Hiba", "JEGYZET Nincs még kész", "Bezárás"),
            //TÖRLÉS
            _ => Shell.Current.DisplayAlert("Hiba", "TÖRLÉS Nincs még kész", "Bezárás"),
        };
    }
}